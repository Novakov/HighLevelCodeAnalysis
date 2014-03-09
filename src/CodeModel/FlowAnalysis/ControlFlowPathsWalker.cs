using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.FlowAnalysis
{
    public abstract class ControlFlowPathsWalker : DepthFirstSearch
    {
        private HashSet<Link> visitedBranchLinks; 

        protected BlockNode EntryPoint { get; private set; }

        protected BlockNode ExitPoint { get; private set; }

        protected MethodInfo Method { get; private set; }

        public virtual void Walk(MethodInfo method, ControlFlowGraph graph)
        {
            this.EntryPoint = graph.EntryPoint;
            this.ExitPoint = graph.ExitPoint;

            this.Method = method;

            this.visitedBranchLinks = new HashSet<Link>();

            base.Walk(graph.EntryPoint);
        }

        protected sealed override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            var links = availableThrough as IList<Link> ?? availableThrough.ToList();

            foreach (var link in links.OfType<ControlTransition>().Where(x => x.Kind == TransitionKind.Backward))
            {
                this.visitedBranchLinks.Add(link);
            }

            this.EnterNode((BlockNode)node, links);
        }

        protected sealed override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.LeaveNode((BlockNode)node, availableThrough);
        }

        protected sealed override IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
        {
            return node.OutboundLinks.Except(this.visitedBranchLinks).GroupBy(x => x.Target);
        }

        protected abstract void EnterNode(BlockNode node, IEnumerable<Link> availableThrough);
        protected abstract void LeaveNode(BlockNode node, IEnumerable<Link> availableThrough);
    }
}