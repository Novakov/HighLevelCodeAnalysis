using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;

namespace CodeModel.FlowAnalysis
{
    public class FindAllControlFlowPaths : FindAllPaths
    {
        private readonly HashSet<Link> visitedBranchLinks;

        public FindAllControlFlowPaths(Node endNode)
            : base(endNode)
        {
            this.visitedBranchLinks = new HashSet<Link>();
        }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            var links = availableThrough as IList<Link> ?? availableThrough.ToList();

            foreach (var link in links.OfType<ControlTransition>().Where(x => x.Kind == TransitionKind.Backward))
            {
                this.visitedBranchLinks.Add(link);
            }

            base.EnterNode(node, links);
        }

        protected override IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
        {
            return node.OutboundLinks.Except(this.visitedBranchLinks).GroupBy(x => x.Target);
        }
    }
}