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
        public List<Tuple<Instruction, PotentialType[]>> Calls { get { return this.recorder.Calls; } }

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
            var nodesCount = controlFlowGraph.Nodes.Count();
            var linksCount = controlFlowGraph.Links.Count();

            while (true)
            {
                ReduceNotImpactingBlocks(controlFlowGraph, method);
                ReduceEmptyPassthroughBlocks(controlFlowGraph);

                var multipleLinks = controlFlowGraph.Links.OfType<ControlTransition>()
                .GroupBy(x => new { x.Source, x.Target })
                .SelectMany(x => x.Skip(1));

                foreach (var controlTransition in multipleLinks)
                {
                    controlFlowGraph.RemoveLink(controlTransition);
                }

                var newNodesCount = controlFlowGraph.Nodes.Count();
                var newLinksCount = controlFlowGraph.Links.Count();

                if (newNodesCount == nodesCount && newLinksCount == linksCount)
                {
                    break;
                }

                nodesCount = newNodesCount;
                linksCount = newLinksCount;
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
