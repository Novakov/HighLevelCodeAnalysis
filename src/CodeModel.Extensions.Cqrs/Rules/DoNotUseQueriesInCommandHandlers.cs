using System.Collections.Generic;
using System.Linq;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.Cqrs.Rules
{
    [Need(CqrsResources.CommandHandlers, CqrsResources.QueryExecutionLinks)]
    public class DoNotUseQueriesInCommandHandlers : INodeRule
    {
        public bool IsApplicableTo(Node node)
        {
            return node is CommandHandlerNode;
        }

        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            foreach (var queryExecutionLink in node.OutboundLinks.OfType<QueryExecutionLink>().GroupBy(x => (QueryNode)x.Target).Select(x => x.Key))
            {
                yield return new CommandHandlerExecutesQueryViolation((CommandHandlerNode) node, (QueryNode) queryExecutionLink);
            }
        }
    }
}