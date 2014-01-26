using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Graphs
{
    public class FindAllPaths : DepthFirstSearch
    {
        private readonly Node endNode;
        public List<IEnumerable<Node>> Paths { get; private set; }
        private readonly Stack<Node> currentPath;
        private readonly List<List<Node>> circularPaths;

        protected FindAllPaths(Node endNode)
        {
            this.endNode = endNode;
            this.currentPath = new Stack<Node>();
            this.Paths = new List<IEnumerable<Node>>();
            this.circularPaths = new List<List<Node>>();
        }

        public static IEnumerable<IEnumerable<Node>> BetweenNodes(Node startNode, Node endNode)
        {
            var search = new FindAllPaths(endNode);

            search.Walk(startNode);

            //search.ResolveCircularPaths();

            return search.Paths;
        }

        private void ResolveCircularPaths()
        {
            while (this.circularPaths.Any())
            {
                var currentPaths = this.Paths.ToList();

                int count = this.circularPaths.Count;

                for (int i = this.circularPaths.Count - 1; i >= 0; i--)
                {
                    bool resolved = false;

                    var circularPath = circularPaths[i];
                    var lastNode = circularPath.Last();

                    var possibleEndings = currentPaths.Where(x => x.Contains(lastNode)).Select(x => x.SkipWhile(y => !y.Equals(lastNode))).Distinct(new PathComparer()).ToList();

                    foreach (var possibleEnding in possibleEndings)
                    {
                        this.Paths.Add(new List<Node>(circularPath.Union(possibleEnding)));
                        resolved = true;
                    }

                    if (resolved)
                    {
                        this.circularPaths.RemoveAt(i);
                    }
                }

                if (count == this.circularPaths.Count)
                {
                    break;
                }
            }
        }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.Push(node);

            if (node.Equals(this.endNode))
            {
                this.Paths.Add(new List<Node>(this.currentPath.Reverse()));
            }
        }

        protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.Pop();
        }

        protected override void AlreadyVisited(Node node, IEnumerable<Link> availableThrough)
        {
            if (!node.Equals(this.endNode))
            {
                this.circularPaths.Add(new List<Node>(this.currentPath.Reverse()));
            }
        }

        private class PathComparer : IEqualityComparer<IEnumerable<Node>>
        {
            public bool Equals(IEnumerable<Node> x, IEnumerable<Node> y)
            {
                return x.Zip(y, (a, b) => x.Equals(y)).All(a => a);
            }

            public int GetHashCode(IEnumerable<Node> obj)
            {
                return obj.Aggregate(1, (a, y) => a * 7 + y.GetHashCode());
            }
        }
    }
}
