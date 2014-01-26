using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
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

        public IEnumerable<IEnumerable<InstructionNode>> FindPaths()
        {
            var paths = new FindAllControlFlowPaths(this.ExitPoint);

            paths.Walk(this.EntryPoint);

            return paths.Paths.Select(x => x.Select(y => (InstructionNode)y));
        }
    }
}