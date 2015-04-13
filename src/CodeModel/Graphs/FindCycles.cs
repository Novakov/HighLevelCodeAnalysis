using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class FindCycles<TNode, TLink>
        where TNode : Node
        where TLink : Link
    {
        public IEnumerable<IEnumerable<TNode>> Find(Graph<TNode, TLink> graph)
        {
            var cycles = new HashSet<IList<TNode>>(new CyclePathComparer());

            foreach (var startNode in graph.Nodes)
            {
                var path = new Stack<TNode>();

                var dfs = new LambdaDepthFirstSearch()
                {
                    EnteringNode = (node, availableThrough) => path.Push((TNode)node),
                    LeavingNode = (node, availableThrough) => path.Pop(),
                    OutboundTargets = node =>
                    {
                        var targets = node.OutboundLinks.OfType<TLink>().GroupBy(x => x.Target).ToList();

                        foreach (var target in targets.ToList())
                        {
                            if (target.Key == startNode)
                            {
                                targets.Remove(target);

                                var cycle = path.Reverse().ToList();
                                cycles.Add(cycle);
                            }
                            else if(path.Contains(target.Key))
                            {
                                targets.Remove(target);
                            }
                        }

                        return targets;
                    }
                };

                dfs.Walk(startNode);
            }

            return cycles;
        }

        public class CyclePathComparer : IEqualityComparer<IList<TNode>>
        {
            public bool Equals(IList<TNode> x, IList<TNode> y)
            {
                if (x.Count != y.Count)
                {
                    return false;
                }

                var offset = y.IndexOf(x[0]);
                if (offset == -1)
                {
                    return false;
                }

                for (int i = 0; i < x.Count; i++)
                {
                    if (y[(i + offset) % x.Count] != x[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(IList<TNode> obj)
            {
                return obj.Count;
            }
        }
    }
}