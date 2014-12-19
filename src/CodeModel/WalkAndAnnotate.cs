using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;

namespace CodeModel
{
    public class WalkAndAnnotate<TNode, TLink> : BreadthFirstSearch<TNode, TLink>
        where TNode : Node
        where TLink : Link
    {
        private readonly Func<TNode, object> nodeAnnotation;
        private readonly Func<TLink, object> linkAnnotation;
        private readonly Func<TNode, IEnumerable<IGrouping<TNode, TLink>>> availableNodes;

        public WalkAndAnnotate(Func<TNode, object> nodeAnnotation, Func<TLink, object> linkAnnotation, Func<TNode, IEnumerable<IGrouping<TNode, TLink>>> availableNodes = null)
        {
            this.nodeAnnotation = nodeAnnotation;
            this.linkAnnotation = linkAnnotation;
            this.availableNodes = availableNodes;
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

        protected override IEnumerable<IGrouping<TNode, TLink>> GetAvailableTargets(TNode @from)
        {
            if (this.availableNodes == null)
            {
                return base.GetAvailableTargets(@from);
            }
            else
            {
                return this.availableNodes(@from);
            }
        }
    }
}