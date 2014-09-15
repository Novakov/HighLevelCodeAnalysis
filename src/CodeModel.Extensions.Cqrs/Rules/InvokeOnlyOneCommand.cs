using System.Linq;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class InvokeOnlyOneCommand : INodeRule
    {
        public const string Category = "InvokeOnlyOneCommand";
        
        public void Verify(VerificationContext context, Node node)
        {
            var methodNode = (MethodNode) node;
            var executedCommands = methodNode.OutboundLinks.OfType<ExecuteCommandLink>();

            if (executedCommands.Count() > 1)
            {
                context.RecordViolation(this, node, Category, null);
            }
        }

        public bool IsApplicableTo(Node node)
        {
            return node is MethodNode;
        }
    }
}