using System;
using System.Collections.Generic;
using CodeModel.Graphs;

namespace CodeModel
{
    public class WalkAndAnnotate<TNode, TLink> : BreadthFirstSearch<TNode, TLink> 
        where TNode : Node 
        where TLink : Link
    {
        private readonly Func<TNode, object> nodeAnnotation;
        private readonly Func<TLink, object> linkAnnotation;

        public WalkAndAnnotate(Func<TNode, object> nodeAnnotation, Func<TLink, object> linkAnnotation)
        {
            this.nodeAnnotation = nodeAnnotation;
            this.linkAnnotation = linkAnnotation;
        }

        public void Walk(Graph<TNode, TLink> graph, TNode start)
        {
            base.WalkCore(graph, start);
        }

        protected override void HandleNode(TNode node, IEnumerable<TLink> availableThrough)
        {
            if (this.nodeAnnotation != null)
            {
                var nodeAnn = this.nodeAnnotation(node);
                if (nodeAnn != null)
                {
                    node.Annonate(nodeAnn);
                }
            }

            if (this.linkAnnotation != null)
            {
                foreach (var link in availableThrough)
                {
                    var linkAnn = this.linkAnnotation(link);
                    if (linkAnn != null)
                    {
                        link.Annonate(this.linkAnnotation(link));
                    }
                }
            }
        }
    }
}