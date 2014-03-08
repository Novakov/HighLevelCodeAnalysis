using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class ComputeStackLength : ControlFlowPathsWalker
    {        
        private int currentStackLength;        

        private Stack<int> diffs;      
        
        public override void Walk(MethodInfo method, ControlFlowGraph cfg)
        {                    
            this.diffs = new Stack<int>();

            base.Walk(cfg.EntryPoint);
        }      

        protected override void EnterNode(BlockNode node, IEnumerable<Link> availableThrough)
        {            
            if (node.Equals(this.ExitPoint))
            {
                if (currentStackLength != 0)
                {
                    throw new InvalidOperationException("Stack length not 0");
                }
            }

            var diff = CalculateDiff(node.Instructions);
            this.diffs.Push(diff);

            this.currentStackLength += diff;
        }

        protected override void LeaveNode(BlockNode node, IEnumerable<Link> availableThrough)
        {
            var diff = this.diffs.Pop();

            this.currentStackLength -= diff;            
        }

        private int CalculateDiff(List<Instruction> instructions)
        {
            return instructions.Aggregate(0, (a, i) => a + i.PushedValuesCount(this.Method) - i.PopedValuesCount(this.Method));
        }        
    }
}
