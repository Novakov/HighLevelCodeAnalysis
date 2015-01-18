using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class LambdaBreadthFirstSearch<TNode, TLink> : BreadthFirstSearch<TNode, TLink>
        where TNode : Node
        where TLink : Link
    {
        public Action<TNode, IEnumerable<TLink>> HandlingNode { get; set; }
        public Func<TNode, IEnumerable<IGrouping<TNode, TLink>>> AvailableTargets { get; set; }

        protected override void HandleNode(TNode node, IEnumerable<TLink> availableThrough)
        {
            if (this.HandlingNode != null)
            {
                this.HandlingNode(node, availableThrough);
            }
        }

        protected override IEnumerable<IGrouping<TNode, TLink>> GetAvailableTargets(TNode @from)
        {
            if (this.AvailableTargets == null)
            {
                return base.GetAvailableTargets(@from);
            }
            else
            {
                return this.AvailableTargets(@from);
            }
        }

        public void Walk(TNode startFrom)
        {
            base.WalkCore(startFrom);
        }
    }
}