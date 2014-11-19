using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs
{
    public class Dupa : IGraphRule
    {
        public const string Category = "a";

        public void Verify(VerificationContext context, Graph graph)
        {
            var v = new PathVerify(context);

            var entryPoint = graph.LookupNode<ApplicationEntryPoint>(ApplicationEntryPoint.NodeId);

            v.Walk(entryPoint);
        }
    }

    class PathVerify : DepthFirstSearch
    {
        private readonly VerificationContext context;
        private readonly Stack<int> commandExecutionCountsOnPath; 

        public PathVerify(VerificationContext context)
        {
            this.context = context;

            this.commandExecutionCountsOnPath = new Stack<int>();
        }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
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
                this.context.RecordViolation(null, node, Dupa.Category, null);
            }
        }

        protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            this.commandExecutionCountsOnPath.Pop();
        }
    }
}