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
        public InstructionNode EntryPoint { get; private set; }

        public ControlFlowGraph(Instruction entrypoint, Instruction exitPoint)
        {
            this.EntryPoint = new InstructionNode(entrypoint);
            this.AddNode(this.EntryPoint);

            this.ExitPoint = new DummyExitPoint();
            // new InstructionNode(exitPoint);
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

            return paths.Paths.Select(x => x.OfType<InstructionNode>());
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