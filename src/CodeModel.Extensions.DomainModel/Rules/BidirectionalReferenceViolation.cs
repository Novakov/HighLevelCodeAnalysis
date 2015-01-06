using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    [Violation(DisplayText = "between {Side1} and {Side2}")]
    public class BidirectionalReferenceViolation : Violation
    {
        public Node Side1 { get; private set; }
        public Node Side2 { get; private set; }

        public BidirectionalReferenceViolation(Node side1, Node side2)
        {
            Side1 = side1;
            Side2 = side2;
        }
    }
}