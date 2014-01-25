using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlow
    {
        private Stack<InstructionNode> remainingInstructions;
        private ControlFlowGraph graph;
        private bool[] visitedInstructions;
        private MethodInfo analyzedMethod;

        public ControlFlowGraph AnalyzeMethod(MethodInfo method)
        {
            this.analyzedMethod = method;
            var instructions = method.GetInstructions();

            var entryPoint = instructions[0];
            var exitPoint = instructions.Last();

            this.graph = new ControlFlowGraph(entryPoint, exitPoint);

            foreach (var instruction in instructions)
            {
                graph.AddNode(new InstructionNode(instruction));
            }

            this.visitedInstructions = new bool[instructions.Last().Offset + 1];

            this.remainingInstructions = new Stack<InstructionNode>();

            this.remainingInstructions.Push(graph.EntryPoint);

            while (this.remainingInstructions.Any())
            {
                var instruction = this.remainingInstructions.Pop();

                if (this.visitedInstructions[instruction.Instruction.Offset])
                {
                    continue;
                }

                this.visitedInstructions[instruction.Instruction.Offset] = true;

                switch (instruction.Instruction.OpCode.FlowControl)
                {
                    case FlowControl.Next:
                    case FlowControl.Call:
                        NextInstruction(instruction);
                        break;
                    case FlowControl.Return:
                        Return(instruction);
                        break;
                    case FlowControl.Cond_Branch:
                        ConditionalBranch(instruction);
                        break;
                    case FlowControl.Branch:
                        UnconditionalBranch(instruction);
                        break;
                    case FlowControl.Throw:
                        Throw(instruction);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unrecognized flow control {0} on instruction {1}", instruction.Instruction.OpCode.FlowControl, instruction.Instruction));
                }

            }

            return graph;
        }

        private void Return(InstructionNode instruction)
        {
            if (instruction.Instruction.OpCode == OpCodes.Ret)
            {
                return;
            }

            if (instruction.Instruction.OpCode == OpCodes.Endfinally)
            {
                var nextNode = this.graph.NodeForInstruction(instruction.Instruction.Next);
                this.graph.AddLink(instruction, nextNode, new ControlTransition());
                this.remainingInstructions.Push(nextNode);

                LinkToThrowTarget(instruction);
            }
        }

        private void Throw(InstructionNode instruction)
        {           
            LinkToThrowTarget(instruction);
        }

        private void LinkToThrowTarget(InstructionNode instruction)
        {
            var body = this.analyzedMethod.GetMethodBody();
            
            var possibleExceptionHandlerBlocks = body.ExceptionHandlingClauses.Where(x => x.TryOffset <= instruction.Instruction.Offset && instruction.Instruction.Offset <= x.TryOffset + x.TryLength);

            foreach (var handlerBlock in possibleExceptionHandlerBlocks)
            {
                var handlerStartNode = this.graph.Nodes.OfType<InstructionNode>().Single(x => x.Instruction.Offset == handlerBlock.HandlerOffset);
                this.graph.AddLink(instruction, handlerStartNode, new ControlTransition());
                this.remainingInstructions.Push(handlerStartNode);
            }          

            this.graph.AddLink(instruction, this.graph.ExitPoint, new ControlTransition());
        }

        private void UnconditionalBranch(InstructionNode instruction)
        {
            var body = analyzedMethod.GetMethodBody();

            var handlingClause = body.ExceptionHandlingClauses.SingleOrDefault(x => instruction.Instruction.Next.Offset == x.TryOffset + x.TryLength);

            if (handlingClause != null && handlingClause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
            {
                var finallyClauseStart = this.graph.Nodes.OfType<InstructionNode>().Single(x => x.Instruction.Offset == handlingClause.HandlerOffset);
                this.graph.AddLink(instruction, finallyClauseStart, new ControlTransition());

                this.remainingInstructions.Push(finallyClauseStart);

                return;
            }

            var target = this.graph.NodeForInstruction((Instruction)instruction.Instruction.Operand);

            graph.AddLink(instruction, target, new ControlTransition());

            this.remainingInstructions.Push(target);
        }

        private void ConditionalBranch(InstructionNode instruction)
        {
            var branchTarget = graph.NodeForInstruction((Instruction)instruction.Instruction.Operand);
            graph.AddLink(instruction, branchTarget, new ControlTransition());

            this.remainingInstructions.Push(branchTarget);

            var next = graph.NodeForInstruction(instruction.Instruction.Next);
            graph.AddLink(instruction, next, new ControlTransition());

            this.remainingInstructions.Push(next);
        }

        private void NextInstruction(InstructionNode instruction)
        {
            var next = this.graph.NodeForInstruction(instruction.Instruction.Next);
            this.graph.AddLink(instruction, next, new ControlTransition());
            this.remainingInstructions.Push(next);
        }
    }

    public class ControlTransition : Link
    {
    }

    public class ControlFlowGraph : Graph
    {
        public InstructionNode ExitPoint { get; private set; }
        public InstructionNode EntryPoint { get; private set; }

        public ControlFlowGraph(Instruction entrypoint, Instruction exitPoint)
        {
            this.EntryPoint = new InstructionNode(entrypoint);
            this.AddNode(this.EntryPoint);

            this.ExitPoint = new InstructionNode(exitPoint);
            this.AddNode(this.ExitPoint);
        }

        public InstructionNode NodeForInstruction(Instruction instruction)
        {
            return this.Nodes.OfType<InstructionNode>().FirstOrDefault(x => x.Instruction == instruction);
        }
    }

    public class InstructionNode : Node
    {
        public Instruction Instruction { get; private set; }

        public InstructionNode(Instruction instruction)
            : base(instruction.ToString())
        {
            this.Instruction = instruction;
        }
    }
}
