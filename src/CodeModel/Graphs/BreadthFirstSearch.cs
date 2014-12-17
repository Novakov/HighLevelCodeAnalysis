using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Graphs
{
    public abstract class BreadthFirstSearch<TNode, TLink>
        where TNode : Node
        where TLink : Link
    {
        protected void WalkCore(Graph<TNode, TLink> graph, TNode startNode)
        {
            var remaining = new Queue<WalkElement>();

            var handled = new HashSet<TNode>();

            remaining.Enqueue(new WalkElement(startNode, Enumerable.Empty<TLink>()));

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

        protected virtual IEnumerable<IGrouping<TNode, TLink>> GetAvailableTargets(TNode from)
        {
            return from.OutboundLinks.OfType<TLink>().GroupBy(x => (TNode)x.Target);
        }

        protected abstract void HandleNode(TNode node, IEnumerable<TLink> availableThrough);

        private class WalkElement
        {
            public TNode Node { get; private set; }
            public IEnumerable<TLink> AvailableThrough { get; private set; }

            public WalkElement(TNode node, IEnumerable<TLink> availableThrough)
            {
                this.Node = node;
                this.AvailableThrough = availableThrough;
            }
        }
    }
}
