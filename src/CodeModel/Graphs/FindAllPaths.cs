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
        private readonly List<Node> currentPath;

        protected FindAllPaths(Node endNode)
        {
            this.endNode = endNode;
            this.currentPath = new List<Node>();
            this.Paths = new List<IEnumerable<Node>>();            
        }

        public static IEnumerable<IEnumerable<Node>> BetweenNodes(Node startNode, Node endNode)
        {
            var search = new FindAllPaths(endNode);

            search.Walk(startNode);
          
            return search.Paths;
        }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.Add(node);

            if (node == this.endNode)
            {
                this.Paths.Add(new List<Node>(this.currentPath));
            }
        }

        protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.currentPath.RemoveAt(this.currentPath.Count - 1);
        }
    }
}
