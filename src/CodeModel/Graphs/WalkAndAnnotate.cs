using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{    
    public class WalkAndAnnotate<TNode, TLink>
        where TNode : Node
        where TLink : Link
    {
        public Func<TNode, object> NodeAnnotation { get; set; }
        public Func<TLink, object> LinkAnnotation { get; set; }
        public Func<TNode, IEnumerable<IGrouping<TNode, TLink>>> AvailableNodes { get; set; }

        public void Walk(Graph<TNode, TLink> graph, TNode start)
        {           
            var bfs = new LambdaBreadthFirstSearch<TNode, TLink>
            {
                HandlingNode = HandleNode,
                AvailableTargets = this.AvailableNodes
            };

            bfs.Walk(start);
        }

        private void HandleNode(TNode node, IEnumerable<TLink> availableThrough)
        {
            if (this.NodeAnnotation != null)
            {
                var nodeAnn = this.NodeAnnotation(node);
                if (nodeAnn != null)
                {
                    node.Annonate(nodeAnn);
                }
            }

            if (this.LinkAnnotation != null)
            {
                foreach (var link in availableThrough)
                {
                    var linkAnn = this.LinkAnnotation(link);
                    if (linkAnn != null)
                    {
                        link.Annonate(this.LinkAnnotation(link));
                    }
                }
            }
        }
    }
}