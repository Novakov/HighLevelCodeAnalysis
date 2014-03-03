﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlow
    {
        private ControlFlowGraph graph;
        private MethodInfo analyzedMethod;

        private const int EndPointOffset = -1;

        public ControlFlowGraph AnalyzeMethod(MethodInfo method)
        {
            this.analyzedMethod = method;
            var instructions = method.GetInstructions();

            var entryPoint = instructions[0];

            this.graph = new ControlFlowGraph(entryPoint);

            var nodes = new InstructionBlockNode[instructions.Last().Offset + 1];

            foreach (var instruction in instructions)
            {
                nodes[instruction.Offset] = (InstructionBlockNode)this.graph.AddNode(new InstructionBlockNode(instruction));
            }

            LinkBlockTransitions(nodes);

            this.graph.RemoveUnreachableBlocks();

            this.graph.ReduceBlocks();

            return graph;
        }

        private void LinkBlockTransitions(IList<InstructionBlockNode> nodes)
        {
            foreach (var node in this.graph.Blocks)
            {
                foreach (var transition in GetTransitions(node.Instructions.FirstOrDefault()))
                {
                    if (transition == EndPointOffset)
                    {
                        this.graph.AddLink(node, this.graph.ExitPoint, new ControlTransition(TransitionKind.Forward));
                    }
                    else
                    {
                        this.graph.AddLink(node, nodes[transition], new ControlTransition(TransitionKindForBranch(node.Instructions[0].Offset, transition)));
                    }
                }
            }
        }

        private IEnumerable<int> Return(Instruction instruction)
        {
            if (instruction.OpCode == OpCodes.Ret)
            {
                return new[] { EndPointOffset };
            }

            if (instruction.OpCode == OpCodes.Endfinally)
            {
                return new[] { instruction.Next.Offset }.Union(LinkToThrowTarget(instruction));
            }

            throw new InvalidOperationException("Unrecognized Return opcode " + instruction);
        }

        private IEnumerable<int> Throw(Instruction instruction)
        {
            return LinkToThrowTarget(instruction);
        }

        private IEnumerable<int> LinkToThrowTarget(Instruction instruction)
        {
            var body = this.analyzedMethod.GetMethodBody();

            var possibleExceptionHandlerBlocks = body.ExceptionHandlingClauses.Where(x => x.TryOffset <= instruction.Offset && instruction.Offset <= x.TryOffset + x.TryLength);

            foreach (var handlerBlock in possibleExceptionHandlerBlocks)
            {
                yield return handlerBlock.HandlerOffset;
            }

            yield return EndPointOffset;
        }

        private IEnumerable<int> UnconditionalBranch(Instruction instruction)
        {
            var body = analyzedMethod.GetMethodBody();

            var handlingClause = body.ExceptionHandlingClauses.OrderBy(x => x.HandlerOffset).FirstOrDefault(x => instruction.Next.Offset == x.TryOffset + x.TryLength);

            if (handlingClause != null && handlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
            {
                return new[] {handlingClause.HandlerOffset};
            }
            else
            {
                return new[] {((Instruction)instruction.Operand).Offset};   
            }            
        }

        private IEnumerable<int> ConditionalBranch(Instruction instruction)
        {
            switch (instruction.OpCode.OperandType)
            {
                case OperandType.InlineSwitch:
                    return JumpTable(instruction);
                default:
                    return new[] { ((Instruction)instruction.Operand).Offset, instruction.Next.Offset };
            }
        }

        private IEnumerable<int> JumpTable(Instruction instruction)
        {
            var targets = ((Instruction[])instruction.Operand);//.Select(x => this.graph.NodeForInstruction(x));

            return targets.Select(x => x.Offset);
        }

        private static TransitionKind TransitionKindForBranch(int fromOffset, int toOffset)
        {
            if (toOffset > fromOffset)
            {
                return TransitionKind.Forward;
            }
            else
            {
                return TransitionKind.Backward;
            }
        }

        private IEnumerable<int> GetTransitions(Instruction instruction)
        {
            switch (instruction.OpCode.FlowControl)
            {
                case FlowControl.Next:
                case FlowControl.Meta:
                case FlowControl.Call:
                    return new[] { instruction.Next.Offset };
                case FlowControl.Return:
                    return Return(instruction);
                case FlowControl.Cond_Branch:
                    return ConditionalBranch(instruction);
                case FlowControl.Branch:
                    return UnconditionalBranch(instruction);
                case FlowControl.Throw:
                    return Throw(instruction);
                default:
                    throw new InvalidOperationException(string.Format("Unrecognized flow control {0} on instruction {1}", instruction.OpCode.FlowControl, instruction));
            }
        }
    }
}
