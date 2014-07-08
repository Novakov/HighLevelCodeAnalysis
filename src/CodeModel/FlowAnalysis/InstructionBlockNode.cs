using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public abstract class BlockNode : Node
    {
        [Exportable]
        public bool IsBranch
        {
            get { return this.OutboundLinks.Count() != 1; }
        }

        [Exportable]
        public bool IsJoin
        {
            get { return this.InboundLinks.Count() != 1; }
        }

        [Exportable]
        public bool IsPassthrough
        {
            get { return !this.IsBranch && !this.IsJoin; }
        }

        [Exportable]
        public int InStackHeight { get; set; }

        [Exportable]
        public int OutStackHeight { get; set; }

        [Exportable]
        public int StackDiff { get; protected set; }

        [Exportable]
        public bool GoesBelowInitialStack { get; protected set; }

        [Exportable]
        public bool SetsLocalVariable { get; protected set; }       

        public List<Instruction> Instructions { get; private set; }

        public IEnumerable<BlockNode> TransitedFrom
        {
            get { return this.InboundLinks.OfType<ControlTransition>().Select(x => (BlockNode)x.Source); }
        }

        public IEnumerable<BlockNode> TransitTo
        {
            get { return this.OutboundLinks.OfType<ControlTransition>().Select(x => (BlockNode)x.Target); }
        }  

        protected BlockNode(string id, params  Instruction[] instructions)
            : base(id)
        {
            this.Instructions = new List<Instruction>(instructions);
        }

        public bool IsBlockBegin()
        {
            return this.IsJoin|| this.TransitedFrom.First().IsBranch;
        }

        internal abstract BlockNode Clone();
        public abstract void CalculateStackProperties(MethodInfo containingMethod);
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

        public override void CalculateStackProperties(MethodInfo containingMethod)
        {
            var methodBody = containingMethod.GetMethodBody();

            int stackValue = 0;
            foreach (var instruction in Instructions)
            {                
                stackValue -= instruction.PopedValuesCount(containingMethod);

                this.GoesBelowInitialStack = this.GoesBelowInitialStack || stackValue < 0;
                this.SetsLocalVariable = this.SetsLocalVariable || instruction.OpCode.IsStoreVariable();

                stackValue += instruction.PushedValuesCount(containingMethod, methodBody);
            }

            this.StackDiff = stackValue;

            this.InStackHeight = this.TransitedFrom.Select(x => (int?)x.OutStackHeight).Distinct().SingleOrDefault() ?? 0;
            this.OutStackHeight = this.InStackHeight + this.StackDiff;            
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

        public override void CalculateStackProperties(MethodInfo containingMethod)
        {
            
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

        public override void CalculateStackProperties(MethodInfo containingMethod)
        {
            
        }
    }
}