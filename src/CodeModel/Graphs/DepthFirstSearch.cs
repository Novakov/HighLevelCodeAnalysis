using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{   
    public abstract class DepthFirstSearch
    {
        public void Walk(Node startNode)
        {          
            WalkCore(startNode, Enumerable.Empty<Link>());
        }

        private void WalkCore(Node node, IEnumerable<Link> availableThrough)
        {
            var links = availableThrough as IList<Link> ?? availableThrough.ToList();

            EnterNode(node, links);

            foreach (var target in GetOutboundTargets(node))
            {
                WalkCore(target.Key, target);
            }

            LeaveNode(node, links);
        }

        protected virtual IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
        {
            return node.OutboundLinks.GroupBy(x => x.Target);
        }

        protected abstract void EnterNode(Node node, IEnumerable<Link> availableThrough);

        protected abstract void LeaveNode(Node node, IEnumerable<Link> availableThrough);
    }
}