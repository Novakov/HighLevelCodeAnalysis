using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class TopologySort
    {
        public static IEnumerable<TNode> SortGraph<TNode, TLink>(Graph<TNode, TLink> graph) 
            where TNode : Node 
            where TLink : Link
        {
            var inboundCount = graph.Nodes.ToDictionary(x => x, x => x.InboundLinks.Count());

            var remainingNodes = new Stack<TNode>(inboundCount.Where(x => x.Value == 0).Select(x => x.Key));

            var sorted = new List<TNode>();

            while (remainingNodes.Any())
            {
                var next = remainingNodes.Pop();
                sorted.Add(next);

                foreach (var outboundLink in next.OutboundLinks)
                {
                    inboundCount[(TNode)outboundLink.Target]--;

                    if (inboundCount[(TNode)outboundLink.Target] == 0)
                    {
                        remainingNodes.Push((TNode)outboundLink.Target);
                    }
                }
            }

            return sorted;
        }
    }
}