using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlowGraph : Graph
    {
        public BlockNode ExitPoint { get; private set; }
        public BlockNode EntryPoint { get; private set; }

        public IEnumerable<BlockNode> Blocks { get { return base.Nodes.OfType<InstructionBlockNode>(); } }

        public ControlFlowGraph(Instruction entrypoint)
        {
            this.EntryPoint = new InstructionBlockNode(entrypoint);
            this.AddNode(this.EntryPoint);

            this.ExitPoint = new MethodExitNode();
            this.AddNode(this.ExitPoint);
        }

        private ControlFlowGraph()
        {
            this.ExitPoint = new MethodExitNode();
            this.AddNode(this.ExitPoint);
        }

        public BlockNode NodeForInstruction(Instruction instruction)
        {
            return this.Blocks.FirstOrDefault(x => x.Instructions.Contains(instruction));
        }

        public IEnumerable<IEnumerable<BlockNode>> FindPaths()
        {
            var paths = new FindAllControlFlowPaths(this.ExitPoint);

            paths.Walk(this.EntryPoint);

            return paths.Paths.Select(x => x.OfType<InstructionBlockNode>());
        }

        public void ReduceBlocks()
        {
            var possibleBlockStarts = this.Blocks.Where(IsBlockBegin).ToList();

            foreach (var possibleBlockStart in possibleBlockStarts)
            {
                ReduceBlock(possibleBlockStart);
            }
        }

        private void ReduceBlock(BlockNode blockStart)
        {
            if (blockStart.IsBranch)
            {
                return;
            }

            var next = blockStart.OutboundLinks.OfType<ControlTransition>().First().Target;

            var nextBlock = next as BlockNode;

            while (nextBlock != null && nextBlock.IsPassthrough)
            {
                blockStart.Instructions.AddRange(((BlockNode)next).Instructions);
                MoveOutboundLinks(next, blockStart);
                RemoveNode(next);

                next = blockStart.OutboundLinks.OfType<ControlTransition>().First().Target;
                nextBlock = next as InstructionBlockNode;
            }

            if (nextBlock != null && nextBlock.IsBranch && !nextBlock.IsJoin)
            {
                blockStart.Instructions.AddRange(((BlockNode)next).Instructions);
                MoveOutboundLinks(next, blockStart);
                RemoveNode(next);
            }
        }

        private bool IsBlockBegin(BlockNode instruction)
        {
            return instruction.IsJoin
                   || instruction.TransitedFrom.First().IsBranch;
        }

        public void RemovePassthroughNode(BlockNode block)
        {
            if (!block.IsPassthrough)
            {
                throw new InvalidOperationException("Cannot remove non passthrough block");
            }

            var inboundLink = block.InboundLinks.Single();
            var outboundLink = block.OutboundLinks.Single();

            this.AddLink(inboundLink.Source, outboundLink.Target, new ControlTransition(TransitionKind.Forward)); // TODO: not sure if this is correct - calculate proper transition kind

            this.RemoveNode(block);
        }

        public void RemoveUnreachableBlocks()
        {
            var unreachable = Nodes.Except(EntryPoint, ExitPoint).Where(x => !x.InboundLinks.Any()).ToList();

            foreach (var unreachableNode in unreachable)
            {
                RemoveNode(unreachableNode);
            }
        }

        public override void ReplaceNode(Node old, Node replaceWith)
        {
            base.ReplaceNode(old, replaceWith);

            if (this.EntryPoint.Equals(old))
            {
                this.EntryPoint = (BlockNode)replaceWith;
            }
        }

        public ControlFlowGraph Clone()
        {
            var copy = new ControlFlowGraph();            

            var map = new Dictionary<string, Node>();

            foreach (var node in this.Nodes.OfType<BlockNode>().Where(x => !(x is MethodExitNode)))
            {
                map[node.Id] = copy.AddNode(node.Clone());
            }

            map[this.ExitPoint.Id] = copy.ExitPoint;

            copy.EntryPoint = (BlockNode) map[this.EntryPoint.Id];

            foreach (var link in this.Links.OfType<ControlTransition>())
            {
                copy.AddLink(copy.Nodes.Single(x => x.Id == link.Source.Id), copy.Nodes.Single(x => x.Id == link.Target.Id), new ControlTransition(link.Kind));
            }

            return copy;
        }

        public void Reduce(params Action<ControlFlowGraph>[] reductors)
        {
            var nodesCount = this.Nodes.Count();
            var linksCount = this.Links.Count();

            while (true)
            {
                foreach (var reductor in reductors)
                {
                    reductor(this);
                }

                var newNodesCount = this.Nodes.Count();
                var newLinksCount = this.Links.Count();

                if (newNodesCount == nodesCount && newLinksCount == linksCount)
                {
                    break;
                }

                nodesCount = newNodesCount;
                linksCount = newLinksCount;
            }
        }
    }
}