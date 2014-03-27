using System;
using System.Collections.Generic;
using CodeModel.Graphs;

namespace CodeModel
{
    public class WalkAndAnnotate : BreadthFirstSearch
    {
        private readonly Func<Node, object> nodeAnnotation;
        private readonly Func<Link, object> linkAnnotation;

        public WalkAndAnnotate(Func<Node, object> nodeAnnotation, Func<Link, object> linkAnnotation)
        {
            this.nodeAnnotation = nodeAnnotation;
            this.linkAnnotation = linkAnnotation;
        }

        public void Walk(Graph graph, Node start)
        {
            base.WalkCore(graph, start);
        }

        protected override void HandleNode(Node node, IEnumerable<Link> availableThrough)
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