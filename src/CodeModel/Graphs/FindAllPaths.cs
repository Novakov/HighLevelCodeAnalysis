using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Graphs
{
    public class FindAllPaths
    {
        public static IEnumerable<IEnumerable<Link>> BetweenNodes(Graph graph, Node startNode, Node endNode)
        {
            var paths = new Dictionary<Node, List<List<Link>>>();

            paths[startNode] = new List<List<Link>>
            {
                new List<Link>()
            };

            var q = new Queue<Node>();
            q.Enqueue(startNode);

            while (q.Any())
            {
                var node = q.Dequeue();

                foreach (var target in node.OutboundLinks.GroupBy(x => x.Target))
                {                                        
                    if (!paths.ContainsKey(target.Key))
                    {
                        paths[target.Key] = new List<List<Link>>();

                        q.Enqueue(target.Key);
                    }

                    foreach (var pathToCurrentNode in paths[node])
                    {
                        foreach (var link in target)
                        {
                            var newPath = new List<Link>(pathToCurrentNode.Union(new[] {link}));

                            paths[target.Key].Add(newPath);
                        }
                    }
                }
            }

            if (paths.ContainsKey(endNode))
            {
                return paths[endNode];
            }
            else
            {
                return Enumerable.Empty<IEnumerable<Link>>();
            }
        }
    }
}
