using System;
using System.Collections.Generic;
using System.Reflection;
using CodeModel.Graphs;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes : ControlFlowPathsWalker
    {
        public List<Tuple<Instruction, PotentialType[]>> Calls { get; private set; }

        private Dupa dupa;

        public override void Walk(MethodInfo method, ControlFlowGraph graph)
        {
            this.Calls = new List<Tuple<Instruction, PotentialType[]>>();

            this.dupa = new Dupa();
            this.dupa.Initialize(method);

            base.Walk(method, graph);
        }

        protected override void EnterNode(BlockNode node, IEnumerable<Link> availableThrough)
        {
            this.dupa.Stack.Mark();

            this.dupa.Walk(node.Instructions);

            this.Calls.AddRange(this.dupa.Calls);
        }

        protected override void LeaveNode(BlockNode node, IEnumerable<Link> availableThrough)
        {
            this.dupa.Stack.Revert();
        }
    }
}
