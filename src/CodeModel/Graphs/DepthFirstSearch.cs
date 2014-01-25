using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public abstract class DepthFirstSearch
    {
        private HashSet<Node> visited;

        public void Walk(Node startNode)
        {
            this.visited = new HashSet<Node>();

            WalkCore(startNode, Enumerable.Empty<Link>());
        }

        private void WalkCore(Node node, IEnumerable<Link> availableThrough)
        {
            var links = availableThrough as IList<Link> ?? availableThrough.ToList();

            EnterNode(node, links);

            if (!this.visited.Contains(node))
            {
                foreach (var target in node.OutboundLinks.GroupBy(x => x.Target))
                {
                    WalkCore(target.Key, target);
                }
            }

            LeaveNode(node, links);                   
        }

        protected abstract void EnterNode(Node node, IEnumerable<Link> availableThrough);

        protected abstract void LeaveNode(Node node, IEnumerable<Link> availableThrough);
    }
}