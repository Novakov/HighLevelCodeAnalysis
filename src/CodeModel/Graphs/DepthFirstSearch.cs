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
               // this.visited.Add(node);

                foreach (var target in GetOutboundTargets(node))
                {
                    WalkCore(target.Key, target);
                }
            }
            else
            {
                AlreadyVisited(node, links);
            }

            LeaveNode(node, links);                   
        }

        protected virtual IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
        {
            return node.OutboundLinks.GroupBy(x => x.Target);
        }

        protected virtual void AlreadyVisited(Node node, IEnumerable<Link> availableThrough)
        {            
        }

        protected abstract void EnterNode(Node node, IEnumerable<Link> availableThrough);

        protected abstract void LeaveNode(Node node, IEnumerable<Link> availableThrough);
    }
}