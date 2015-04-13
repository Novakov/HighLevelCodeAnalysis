using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class TopologySort
    {
        public static IEnumerable<TNode> SortGraph<TNode, TLink>(Graph<TNode, TLink> graph, Func<TNode, IEnumerable<Link>> inLinks = null, Func<TNode, IEnumerable<Link>> outLinks = null) 
            where TNode : Node 
            where TLink : Link
        {
            if (inLinks == null)
            {
                inLinks = n => n.InboundLinks;
            }

            if (outLinks == null)
            {
                outLinks = n => n.OutboundLinks;
            }

            var inboundCount = graph.Nodes.ToDictionary(x => x, x => inLinks(x).Count());

            var remainingNodes = new Stack<TNode>(inboundCount.Where(x => x.Value == 0).Select(x => x.Key));

            var sorted = new List<TNode>();

            while (remainingNodes.Any())
            {
                var next = remainingNodes.Pop();
                sorted.Add(next);

                foreach (var outboundLink in outLinks(next))
                {
                    inboundCount[(TNode)outboundLink.Target]--;

                    if (inboundCount[(TNode)outboundLink.Target] == 0)
                    {
                        remainingNodes.Push((TNode)outboundLink.Target);
                    }
                }
            }

            if (sorted.Count != graph.Nodes.Count())
            {
                throw new CannotSortGraphException(graph.Nodes.Except(sorted).ToList(), sorted);
            }

            return sorted;
        }
    }
}