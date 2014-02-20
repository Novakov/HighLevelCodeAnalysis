using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class InstructionBlockNode : Node
    {
        public List<Instruction> Instructions { get; private set; }

        public Instruction First { get { return this.Instructions[0]; } }
        public Instruction Last { get { return this.Instructions.Last(); } }

        public IEnumerable<InstructionBlockNode> TransitedFrom
        {
            get { return this.InboundLinks.OfType<ControlTransition>().Select(x => (InstructionBlockNode) x.Source); }
        }

        public bool IsBranch
        {
            get { return this.OutboundLinks.Count() != 1; }
        }

        public bool IsJoin
        {
            get { return this.InboundLinks.Count() != 1; }
        }

        public bool IsPassthrough
        {
            get { return !this.IsBranch && !this.IsJoin; }
        }

        public InstructionBlockNode(params Instruction[] instructions)
            : base(instructions.First().ToString())
        {
            this.Instructions = new List<Instruction>(instructions);
        }

        public override string ToString()
        {
            return "[" + First.ToString() + ":" + Last.ToString() + "]";
        }

        public override string DisplayLabel
        {
            get { return this.ToString(); }
        }
    }
}