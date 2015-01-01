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

        public IEnumerable<BlockNode> Blocks { get { return base.Nodes.OfType<InstructionBlockNode>(); } }

        public ControlFlowGraph(BlockNode entryPoint)
        {
            this.EntryPoint = entryPoint;
            this.AddNode(entryPoint);

            this.ExitPoint = new MethodExitNode();
            this.AddNode(this.ExitPoint);
        }

        public BlockNode BlockWithInstruction(Instruction instruction)
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

        public override void ReplaceNode(BlockNode old, BlockNode replaceWith)
        {
            base.ReplaceNode(old, replaceWith);

            if (this.EntryPoint.Equals(old))
            {
                this.EntryPoint = replaceWith;
            }
        }
    }
}