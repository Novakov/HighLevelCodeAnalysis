using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class UnusedCommandViolation : Violation, INodeViolation
    {
        public Node Node { get; private set; }

        public UnusedCommandViolation(CommandNode commandNode)
        {
            Node = commandNode;
        }
    }
}