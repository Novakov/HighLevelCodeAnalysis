using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public enum TopologySortDirection
    {
        ReduceInbound,
        ReduceOutbound
    }

    public class TopologySort
    {
        public static IEnumerable<TNode> SortGraph<TNode, TLink>(Graph<TNode, TLink> graph, TopologySortDirection direction = TopologySortDirection.ReduceInbound)
            where TNode : Node
            where TLink : Link
        {
            Func<TNode, IEnumerable<TLink>> linksToReduce;
            Func<TNode, IEnumerable<TLink>> linksThatReduce;
            Func<TLink, TNode> reducedNode;

            switch (direction)
            {
                case TopologySortDirection.ReduceInbound:
                    linksToReduce = n => n.InboundLinks.OfType<TLink>();
                    linksThatReduce = n => n.OutboundLinks.OfType<TLink>();
                    reducedNode = l => (TNode)l.Target;
                    break;
                case TopologySortDirection.ReduceOutbound:
                    linksToReduce = n => n.OutboundLinks.OfType<TLink>();
                    linksThatReduce = n => n.InboundLinks.OfType<TLink>();
                    reducedNode = l => (TNode)l.Source;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            var inboundCount = graph.Nodes.ToDictionary(x => x, x => linksToReduce(x).Count());

            var remainingNodes = new Stack<TNode>(inboundCount.Where(x => x.Value == 0).Select(x => x.Key));

            var sorted = new List<TNode>();

            while (remainingNodes.Any())
            {
                var next = remainingNodes.Pop();
                sorted.Add(next);

                foreach (var reducedLink in linksThatReduce(next))
                {
                    var target = reducedNode(reducedLink);
                    inboundCount[target]--;

                    if (inboundCount[target] == 0)
                    {
                        remainingNodes.Push(target);
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