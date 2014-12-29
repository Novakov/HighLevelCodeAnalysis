using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlowGraph : Graph<BlockNode, ControlTransition>
    {
        public BlockNode ExitPoint { get; private set; }
        public BlockNode EntryPoint { get; private set; }

        public static readonly Action<ControlFlowGraph> RemoveUnreachableBlocksReductor = cfg => cfg.RemoveUnreachableBlocks();
        public static readonly Action<ControlFlowGraph> MergePassthroughBlocksReductor = cfg => cfg.MergePassthroughBlocks();

        public IEnumerable<BlockNode> Blocks { get { return base.Nodes.OfType<InstructionBlockNode>(); } }

        public ControlFlowGraph(BlockNode entryPoint)
        {
            this.EntryPoint = entryPoint;
            this.AddNode(entryPoint);

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

        //TODO: Check if this method is needed. Calculating all paths is too complex
        public IEnumerable<IEnumerable<BlockNode>> FindPaths()
        {
            var paths = new FindAllControlFlowPaths(this.ExitPoint);

            paths.Walk(this.EntryPoint);

            return paths.Paths.Select(x => x.OfType<InstructionBlockNode>());
        }

        public void MergePassthroughBlocks()
        {
            var possibleBlockStarts = this.Blocks.Where(x => x.IsBlockBegin() && !x.IsBranch).ToList();

            foreach (var possibleBlockStart in possibleBlockStarts)
            {
                var next = (BlockNode)possibleBlockStart.OutboundLinks.OfType<ControlTransition>().First().Target;

                var nextBlock = next;

                while (nextBlock != null && nextBlock.IsPassthrough)
                {
                    possibleBlockStart.Instructions.AddRange(next.Instructions);
                    MoveOutboundLinks(next, possibleBlockStart);
                    RemoveNode(next);

                    next = (BlockNode)possibleBlockStart.OutboundLinks.OfType<ControlTransition>().First().Target;
                    nextBlock = next as InstructionBlockNode;
                }

                if (nextBlock != null && nextBlock.IsBranch && !nextBlock.IsJoin)
                {
                    possibleBlockStart.Instructions.AddRange(((BlockNode)next).Instructions);
                    MoveOutboundLinks(next, possibleBlockStart);
                    RemoveNode(next);
                }
            }
        }

        public void RemovePassthroughNode(BlockNode block)
        {
            if (!block.IsPassthrough)
            {
                throw new InvalidOperationException("Cannot remove non passthrough block");
            }

            var inboundLink = block.InboundLinks.Single();
            var outboundLink = block.OutboundLinks.Single();

            this.AddLink((BlockNode)inboundLink.Source, (BlockNode)outboundLink.Target, new ControlTransition(TransitionKind.Forward)); // TODO: not sure if this is correct - calculate proper transition kind

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

        public override void ReplaceNode(BlockNode old, BlockNode replaceWith)
        {
            base.ReplaceNode(old, replaceWith);

            if (this.EntryPoint.Equals(old))
            {
                this.EntryPoint = replaceWith;
            }
        }

        public ControlFlowGraph Clone()
        {
            var copy = new ControlFlowGraph();

            var map = new Dictionary<string, Node>();

            foreach (var node in this.Nodes.Where(x => !(x is MethodExitNode)))
            {
                map[node.Id] = copy.AddNode(node.Clone());
            }

            map[this.ExitPoint.Id] = copy.ExitPoint;

            copy.EntryPoint = (BlockNode)map[this.EntryPoint.Id];

            foreach (var link in this.Links)
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