using CodeModel.Graphs;
using CodeModel.RuleEngine;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodExecutesMoreThanOneCommandViolation : Violation, INodeViolation
    {
        public Node Node { get; private set; }

        public MethodExecutesMoreThanOneCommandViolation(Node node)           
        {
            this.Node = node;
        }
    }
}