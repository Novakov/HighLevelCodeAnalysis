using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.Cqrs.Rules
{
    [Violation(DisplayText = "Command handler {Node} executes query {Query}")]
    public class CommandHandlerExecutesQueryViolation : Violation, INodeViolation
    {
        public Node Node { get; private set; }
        public QueryNode Query { get; private set; }

        public CommandHandlerExecutesQueryViolation(CommandHandlerNode commandHandler, QueryNode query)
        {
            Node = commandHandler;
            Query = query;
        }
    }
}