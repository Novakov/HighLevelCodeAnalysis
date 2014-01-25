using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Graphs
{
    public abstract class BreadthFirstSearch
    {
        public void Walk(Graph graph, Node startNode)
        {
            var remaining = new Queue<Node>();

            var handled = new HashSet<Node>();

            remaining.Enqueue(startNode);

            while (remaining.Any())
            {
                var node = remaining.Dequeue();

                handled.Add(node);

                HandleNode(node);

                foreach (var next in node.OutboundLinks.Select(x => x.Target).Distinct())
                {
                    if (!handled.Contains(next))
                    {
                        remaining.Enqueue(next);
                    }
                }
            }
        }

        protected abstract void HandleNode(Node node);
    }
}
