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
        private readonly List<IEnumerable<Node>> paths;
        private readonly Stack<Node> currentPath; 

        private FindAllPaths(Node endNode)
        {
            this.endNode = endNode;
            this.currentPath = new Stack<Node>();
            this.paths = new List<IEnumerable<Node>>();
        }

        public static IEnumerable<IEnumerable<Node>> BetweenNodes(Graph graph, Node startNode, Node endNode)
        {
            var search = new FindAllPaths(endNode);

            search.Walk(startNode);
            
            return search.paths;
        }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.Push(node);

            if (node.Equals(this.endNode))
            {
                this.paths.Add(new List<Node>(this.currentPath.Reverse()));
            }
        }

        protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.Pop();
        }
    }
}
