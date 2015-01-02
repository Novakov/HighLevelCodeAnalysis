using CodeModel.Graphs;
using CodeModel.RuleEngine;
using CodeModel.Symbols;

namespace CodeModel.Rules
{
    public class UsesDateTimeNowViolation : Violation, IViolationWithSourceLocation, INodeViolation
    {
        public SourceLocation? SourceLocation { get; private set; }
        public Node Node { get; private set; }

        public UsesDateTimeNowViolation(Node node, SourceLocation? sourceLocation)
        {
            this.Node = node;
            this.SourceLocation = sourceLocation;
        }
    }
}