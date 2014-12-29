using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{    
    public class ControlFlowGraphFactory
    {
        private ControlFlowGraph graph;
        private readonly MethodInfo analyzedMethod;

        private ControlFlowGraphFactory(MethodInfo analyzedMethod)
        {
            this.analyzedMethod = analyzedMethod;
        }

        internal const int EndPointOffset = -1;

        // Convert to extension method for MethodInfo
        public static ControlFlowGraph BuildForMethod(MethodInfo method)
        {
            return new ControlFlowGraphFactory(method).Build();
        }

        private ControlFlowGraph Build()
        {
            var body = this.analyzedMethod.GetMethodBody();

            var instructions = this.analyzedMethod.GetInstructions();

            var previousInstructions = instructions.ToDictionary(x => x.Offset, x => x.Previous);
            previousInstructions[EndPointOffset] = instructions.Last();

            var byOffset = instructions.ToDictionary(x => x.Offset, x => x);

            var branchPoints = new HashSet<int>();
            var joinPoints = new HashSet<int>();

            var transitions = new List<Tuple<int, int>>();

            foreach (var instruction in instructions)
            {
                var transistionsFromInstruction = GetTransitions(instruction).ToArray();

                if (IsBlockBoundary(instruction))
                {
                    joinPoints.UnionWith(transistionsFromInstruction);
                    branchPoints.UnionWith(transistionsFromInstruction.Where(x => x != 0).Select(x => previousInstructions[x].Offset));

                    branchPoints.Add(instruction.Offset);
                }

                transitions.AddRange(transistionsFromInstruction.Select(x => Tuple.Create(instruction.Offset, x)));
            }

            foreach (var clause in body.ExceptionHandlingClauses)
            {
                if (clause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                {
                    joinPoints.Add(clause.HandlerOffset);

                    var lastInstructionInTry = previousInstructions[clause.TryOffset + clause.TryLength].Offset;
                    transitions.Add(Tuple.Create(lastInstructionInTry, clause.HandlerOffset));
                    transitions.Remove(Tuple.Create(lastInstructionInTry, clause.HandlerOffset + clause.HandlerLength));
                }

                if (clause.Flags.HasFlag(ExceptionHandlingClauseOptions.Clause))
                {
                    joinPoints.Add(clause.HandlerOffset);
                }

                if (clause.Flags.HasFlag(ExceptionHandlingClauseOptions.Fault))
                {
                    joinPoints.Add(clause.HandlerOffset);                    
                }
            }

            var blockBoundaries = new List<int>();
            blockBoundaries.AddRange(joinPoints);
            blockBoundaries.AddRange(branchPoints);

            blockBoundaries.Remove(EndPointOffset);

            if (!blockBoundaries.Contains(0))
            {
                blockBoundaries.Add(0);
            }

            if (blockBoundaries.Count % 2 == 1)
            {
                blockBoundaries.Add(0);
            }

            blockBoundaries.Sort(new OffsetComparer());

            var blockBoundary = blockBoundaries.GetEnumerator();

            this.graph = null;

            var byBoundary = new Dictionary<int, BlockNode>();

            while (blockBoundary.MoveNext())
            {
                var fromOffset = blockBoundary.Current;
                blockBoundary.MoveNext();
                var toOffset = blockBoundary.Current;

                if (toOffset == EndPointOffset)
                {
                    toOffset = instructions.Last().Offset;
                }

                var instructionsInBlock = byOffset.Where(x => fromOffset <= x.Key && x.Key <= toOffset).Select(x => x.Value);

                var block = new InstructionBlockNode(instructionsInBlock.ToArray());

                if (this.graph == null)
                {
                    this.graph = new ControlFlowGraph(block);
                }
                else
                {
                    this.graph.AddNode(block);
                }

                byBoundary[fromOffset] = block;
                byBoundary[toOffset] = block;
            }

            byBoundary[EndPointOffset] = this.graph.ExitPoint;

            var transitionsBetweenBlocks = transitions.Where(x => x.Item2 == EndPointOffset || (blockBoundaries.Contains(x.Item1) && blockBoundaries.Contains(x.Item2)));

            foreach (var transition in transitionsBetweenBlocks)
            {
                var from = byBoundary[transition.Item1];
                var to = byBoundary[transition.Item2];

                if (from.Equals(to) && transition.Item1 <= transition.Item2)
                {
                    continue;
                }

                this.graph.AddLink(from, to, new ControlTransition(TransitionKindForBranch(transition.Item1, transition.Item2)));
            }

            foreach (var block in this.graph.Blocks)
            {
                block.CalculateStackProperties(this.analyzedMethod);
            }

            return this.graph;
        }

        private static bool IsBlockBoundary(Instruction instruction)
        {
            switch (instruction.OpCode.FlowControl)
            {
                case FlowControl.Next:
                case FlowControl.Meta:
                case FlowControl.Call:
                    return false;
                case FlowControl.Return:
                case FlowControl.Cond_Branch:
                case FlowControl.Branch:
                case FlowControl.Throw:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("instruction", "Cannot determine if instruction " + instruction + " is block boundary");
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
            return new[] { ((Instruction)instruction.Operand).Offset };
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
            var targets = ((Instruction[])instruction.Operand);

            return targets.Select(x => x.Offset).Union(new[] { instruction.Next.Offset });
        }

        private static TransitionKind TransitionKindForBranch(int fromOffset, int toOffset)
        {
            if (toOffset > fromOffset || toOffset == EndPointOffset)
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

    internal class OffsetComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x == ControlFlowGraphFactory.EndPointOffset && y == ControlFlowGraphFactory.EndPointOffset)
            {
                return 0;
            }

            if (x == ControlFlowGraphFactory.EndPointOffset)
            {
                return 1;
            }

            if (y == ControlFlowGraphFactory.EndPointOffset)
            {
                return -1;
            }

            return Comparer<int>.Default.Compare(x, y);
        }
    }
}
