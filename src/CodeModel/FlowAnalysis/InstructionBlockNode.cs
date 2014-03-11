using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public abstract class BlockNode : Node
    {
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

        public List<Instruction> Instructions { get; private set; }

        public IEnumerable<BlockNode> TransitedFrom
        {
            get { return this.InboundLinks.OfType<ControlTransition>().Select(x => (BlockNode)x.Source); }
        }   

        protected BlockNode(string id, params  Instruction[] instructions)
            : base(id)
        {
            this.Instructions = new List<Instruction>(instructions);
        }

        internal abstract BlockNode Clone();
    }

    public class InstructionBlockNode : BlockNode
    {        
        public Instruction First { get { return this.Instructions[0]; } }
        public Instruction Last { get { return this.Instructions.Last(); } }        

        public InstructionBlockNode(params Instruction[] instructions)
            : base(instructions.First().ToString(), instructions)
        {            
        }

        public override string ToString()
        {
            return "[" + First.ToString() + ":" + Last.ToString() + "]";
        }

        public override string DisplayLabel
        {
            get { return this.ToString(); }
        }

        internal override BlockNode Clone()
        {
            return new InstructionBlockNode(this.Instructions.ToArray());
        }
    }

    public class MethodExitNode : BlockNode
    {
        public MethodExitNode()
            : base("exit-point")
        {
            
        }

        internal override BlockNode Clone()
        {
            return new MethodExitNode();
        }
    }

    internal class EmptyBlock : BlockNode
    {
        public EmptyBlock(string id)
            : base(id)
        {
        }

        public override string ToString()
        {
            return "Empty block";
        }

        public override string DisplayLabel
        {
            get { return "Empty block"; }
        }

        internal override BlockNode Clone()
        {
            return new EmptyBlock(this.Id);
        }
    }
}