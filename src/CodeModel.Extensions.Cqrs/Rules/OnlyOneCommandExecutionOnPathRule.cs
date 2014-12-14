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
        public void Verify(VerificationContext context, Graph graph)
        {
            var v = new PathVerify(context);

            var entryPoint = graph.LookupNode<ApplicationEntryPoint>(ApplicationEntryPoint.NodeId);

            v.Walk(entryPoint);
        }

        public class PathVerify : DepthFirstSearch
        {
            private readonly VerificationContext context;
            private readonly HashSet<Link> visitedLinks;
            private readonly Stack<PathItem> currentPath;

            public PathVerify(VerificationContext context)
            {
                this.context = context;

                this.visitedLinks = new HashSet<Link>();
                this.currentPath = new Stack<PathItem>();
            }

            protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
            {
                this.visitedLinks.UnionWith(availableThrough);

                var item = new PathItem(node);

                foreach (var pathItem in this.currentPath)
                {
                    pathItem.Increment(item.CommandExecutionCount);
                }

                this.currentPath.Push(item);
            }

            protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
            {
                var item = this.currentPath.Pop();

                if (item.CommandExecutionCount > 1)
                {
                    this.context.RecordViolation(new MethodCanLeadToExecutionOfMoreThanOneCommandViolation(null, node)
                        .Attach("path", this.currentPath.Select(x => x.Node).ToList()));
                }
            }

            protected override IEnumerable<IGrouping<Node, Link>> GetOutboundTargets(Node node)
            {
                return node.OutboundLinks
                    .Where(x => x is MethodCallLink || x is ExecuteCommandLink || x is ApplicationEntryCall)
                    .Except(this.visitedLinks)
                    .GroupBy(x => x.Target);
            }

            private class PathItem
            {
                public Node Node { get; private set; }
                public int CommandExecutionCount { get; private set; }

                public PathItem(Node node)
                {
                    this.Node = node;
                    var count = node.Annotation<CommandExecutionCount>();
                    if (count != null)
                    {
                        this.CommandExecutionCount = count.HighestCount;
                    }
                    else
                    {
                        this.CommandExecutionCount = 0;
                    }
                }

                public void Increment(int count)
                {
                    this.CommandExecutionCount += count;
                }
            }
        }
    }

    public class MethodCanLeadToExecutionOfMoreThanOneCommandViolation : Violation
    {
        public const string ViolationCategory = "OnlyOneCommandExecutionOnPathRule";

        public MethodCanLeadToExecutionOfMoreThanOneCommandViolation(OnlyOneCommandExecutionOnPathRule rule, Node node)
            : base(rule, node, ViolationCategory, null)
        {
            
        }
    }
}