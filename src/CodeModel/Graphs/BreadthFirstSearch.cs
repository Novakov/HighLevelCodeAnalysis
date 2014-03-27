using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Graphs
{
    public abstract class BreadthFirstSearch
    {
        protected void WalkCore(Graph graph, Node startNode)
        {
            var remaining = new Queue<WalkElement>();

            var handled = new HashSet<Node>();

            remaining.Enqueue(new WalkElement(startNode, Enumerable.Empty<Link>()));

            while (remaining.Any())
            {
                var node = remaining.Dequeue();

                handled.Add(node.Node);

                HandleNode(node.Node, node.AvailableThrough);

                foreach (var next in GetAvailableTargets(node.Node))
                {
                    if (!handled.Contains(next.Key))
                    {
                        remaining.Enqueue(new WalkElement(next.Key, next));
                    }
                }
            }
        }

        protected virtual IEnumerable<IGrouping<Node, Link>> GetAvailableTargets(Node from)
        {
            return from.OutboundLinks.GroupBy(x => x.Target);
        }

        protected abstract void HandleNode(Node node, IEnumerable<Link> availableThrough);

        private class WalkElement
        {
            public Node Node { get; private set; }
            public IEnumerable<Link> AvailableThrough { get; private set; }

            public WalkElement(Node node, IEnumerable<Link> availableThrough)
            {
                this.Node = node;
                this.AvailableThrough = availableThrough;
            }
        }
    }
}
