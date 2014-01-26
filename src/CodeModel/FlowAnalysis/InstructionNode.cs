using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class InstructionNode : Node
    {
        public Instruction Instruction { get; private set; }        

        public InstructionNode(Instruction instruction)
            : base(instruction.ToString())
        {
            this.Instruction = instruction;
        }

        internal InstructionNode(string id)
            : base(id)
        {
        }
    }
}