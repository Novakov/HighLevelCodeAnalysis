using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class OnlyOneCommandExecutionOnPathRule : IGraphRule
    {
        public const string Category = "OnlyOneCommandExecutionOnPathRule";

        public void Verify(VerificationContext context, Graph graph)
        {
            var v = new PathVerify(context);

            var entryPoint = graph.LookupNode<ApplicationEntryPoint>(ApplicationEntryPoint.NodeId);

            v.Walk(entryPoint);
        }

        public class PathVerify : DepthFirstSearch
        {
            private readonly VerificationContext context;
            private readonly Stack<int> commandExecutionCountsOnPath;
            private readonly HashSet<Link> visitedLinks;
            private Stack<Node> currentPath;

            public PathVerify(VerificationContext context)
            {
                this.context = context;

                this.commandExecutionCountsOnPath = new Stack<int>();

                this.visitedLinks = new HashSet<Link>();
                this.currentPath = new Stack<Node>();
            }

            protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
            {
                this.visitedLinks.UnionWith(availableThrough);

                this.currentPath.Push(node);

                var count = node.Annotation<CommandExecutionCount>();

                if (count != null)
                {
                    this.commandExecutionCountsOnPath.Push(count.HighestCount);
                }
                else
                {
                    this.commandExecutionCountsOnPath.Push(0);
                }

                if (this.commandExecutionCountsOnPath.Sum() > 1)
                {
                    this.context.RecordViolation(null, node, OnlyOneCommandExecutionOnPathRule.Category, null)
                        .Attach("path", this.currentPath.ToList());
                }
            }

            protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
            {
                this.commandExecutionCountsOnPath.Pop();
                this.currentPath.Pop();
            }

            protected override IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
            {
                return node.OutboundLinks
                    .Where(x => x is MethodCallLink || x is ExecuteCommandLink || x is ApplicationEntryCall)
                    .Except(this.visitedLinks)
                    .GroupBy(x => x.Target);
            }
        }
    }
}