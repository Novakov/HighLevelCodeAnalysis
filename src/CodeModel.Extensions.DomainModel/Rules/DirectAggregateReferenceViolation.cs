using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    public class DirectAggregateReferenceViolation : Violation, INodeViolation
    {
        public Node Node { get; private set; }
        public AggregateNode ReferencedAggregate { get; private set; }

        public DirectAggregateReferenceViolation(Node node, AggregateNode referencedAggregate)
        {
            Node = node;
            ReferencedAggregate = referencedAggregate;
        }
    }
}