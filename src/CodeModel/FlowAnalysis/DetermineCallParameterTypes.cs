using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes : ControlFlowPathsWalker
    {
        public IDictionary<MethodBase, HashSet<PotentialType[]>> Calls { get { return this.recorder.Calls; } }

        private CallParameterTypesRecorder recorder;

        public override void Walk(MethodInfo method, ControlFlowGraph graph)
        {            
            this.recorder = new CallParameterTypesRecorder();
            this.recorder.Initialize(method);

            var reducedGraph = graph.Clone();

            Reduce(method, reducedGraph);

            base.Walk(method, reducedGraph);
        }

        public static void Reduce(MethodInfo method, ControlFlowGraph controlFlowGraph)
        {
            foreach (var blockNode in controlFlowGraph.Blocks.OfType<InstructionBlockNode>())
            {
                blockNode.CalculateStackProperties(method);
            }

            var reductors = new Action<ControlFlowGraph>[]
            {
                cfg => ReduceNotImpactingBlocks(cfg, method),
                ReduceEmptyPassthroughBlocks,
                RemoveDuplicatedLinks,
                cfg => ReduceBranchesWithNoImpact(cfg, method),
                ControlFlowGraph.MergePassthroughBlocksReductor,
            };

            controlFlowGraph.Reduce(reductors);           
        }

        private static void RemoveDuplicatedLinks(ControlFlowGraph controlFlowGraph)
        {
            var multipleLinks = controlFlowGraph.Links.OfType<ControlTransition>()
                .GroupBy(x => new {x.Source, x.Target})
                .SelectMany(x => x.Skip(1));

            foreach (var controlTransition in multipleLinks)
            {
                controlFlowGraph.RemoveLink(controlTransition);
            }
        }

        private static void ReduceEmptyPassthroughBlocks(ControlFlowGraph cfg)
        {
            var remainingBlocks = new Queue<BlockNode>(cfg.Nodes.OfType<BlockNode>());

            while (remainingBlocks.Any())
            {
                var block = remainingBlocks.Dequeue() as EmptyBlock;
                if (block == null)
                {
                    continue;
                }

                if (block.IsPassthrough)
                {
                    cfg.RemovePassthroughNode(block);
                }
            }
        }

        private static void ReduceNotImpactingBlocks(ControlFlowGraph cfg, MethodInfo indexMethod)
        {
            var remainingBlocks = new Queue<BlockNode>(cfg.Blocks);

            var methodBody = indexMethod.GetMethodBody();
            while (remainingBlocks.Any())
            {
                var block = remainingBlocks.Dequeue();

                if (!HasImpact(block, indexMethod, methodBody))
                {
                    cfg.ReplaceNode(block, new EmptyBlock(block.Instructions.First().ToString()));
                }
            }
        }

        private static void ReduceBranchesWithNoImpact(ControlFlowGraph controlFlowGraph, MethodInfo containingMethod)
        {           
            foreach (var block in controlFlowGraph.Blocks.OfType<InstructionBlockNode>())
            {
                if (!block.IsPassthrough)
                {
                    continue;
                }

                if (block.GoesBelowInitialStack || block.SetsLocalVariable)
                {
                    continue;
                }

                var branchBlock = block.TransitedFrom.Single();
                var joinBlock = block.TransitTo.Single();

                var bypassingTransition = branchBlock.OutboundLinks.Where(x => x.Target.Equals(joinBlock));

                if (bypassingTransition.Count() == 1)
                {
                    controlFlowGraph.RemoveLink(bypassingTransition.Single());
                }
            }
        }

        private static bool HasImpact(BlockNode block, MethodInfo method, MethodBody methodBody)
        {
            int stackLength = 0;

            foreach (var instruction in block.Instructions)
            {
                stackLength -= instruction.PopedValuesCount(method);

                if (stackLength < 0)
                {
                    return true;
                }

                if (instruction.OpCode.IsStoreVariable() || instruction.OpCode == OpCodes.Starg || instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                {
                    return true;
                }

                stackLength += instruction.PushedValuesCount(method, methodBody);
            }

            return stackLength != 0;
        }

        protected override void EnterNode(BlockNode node, IEnumerable<Link> availableThrough)
        {
            this.recorder.Stack.Mark();

            this.recorder.Visit(node.Instructions);            
        }

        protected override void LeaveNode(BlockNode node, IEnumerable<Link> availableThrough)
        {
            this.recorder.Stack.Revert();
        }
    }
}
