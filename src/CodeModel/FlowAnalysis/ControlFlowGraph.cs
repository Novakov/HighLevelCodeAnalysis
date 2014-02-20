using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlowGraph : Graph
    {
        public Node ExitPoint { get; private set; }
        public InstructionBlockNode EntryPoint { get; private set; }

        public IEnumerable<InstructionBlockNode> Blocks { get { return base.Nodes.OfType<InstructionBlockNode>(); } }

        public ControlFlowGraph(Instruction entrypoint)
        {
            this.EntryPoint = new InstructionBlockNode(entrypoint);
            this.AddNode(this.EntryPoint);

            this.ExitPoint = new DummyExitPoint();
            this.AddNode(this.ExitPoint);
        }

        public InstructionBlockNode NodeForInstruction(Instruction instruction)
        {
            return this.Blocks.FirstOrDefault(x => x.Instructions.Contains(instruction));
        }

        public IEnumerable<IEnumerable<InstructionBlockNode>> FindPaths()
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

        private void ReduceBlock(InstructionBlockNode blockStart)
        {
            if (blockStart.IsBranch)
            {
                return;
            }

            var next = blockStart.OutboundLinks.OfType<ControlTransition>().First().Target;

            var nextBlock = next as InstructionBlockNode;

            while (nextBlock != null && nextBlock.IsPassthrough)
            {
                blockStart.Instructions.AddRange(((InstructionBlockNode) next).Instructions);
                MoveOutboundLinks(next, blockStart);
                RemoveNode(next);

                next = blockStart.OutboundLinks.OfType<ControlTransition>().First().Target;
                nextBlock = next as InstructionBlockNode;
            }

            if (nextBlock != null && nextBlock.IsBranch && !nextBlock.IsJoin)
            {
                blockStart.Instructions.AddRange(((InstructionBlockNode)next).Instructions);
                MoveOutboundLinks(next, blockStart);
                RemoveNode(next);
            }
        }       

        public bool IsBlockBegin(InstructionBlockNode instruction)
        {
            return instruction.IsJoin
                   || instruction.TransitedFrom.First().IsBranch;
        }

        private class DummyExitPoint : Node
        {
            public DummyExitPoint()
                : base("exit-point")
            {

            }
        }
    }
}