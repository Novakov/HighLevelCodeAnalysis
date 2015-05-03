using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    [Violation(DisplayText = "Field {ViolatingField} is writable")]
    public class ImmutableTypeHasWritableFieldViolation : Violation, INodeViolation
    {
        public FieldNode ViolatingField { get; private set; }
        public Node Node { get; private set; }

        public ImmutableTypeHasWritableFieldViolation(TypeNode violatingType, FieldNode violatingField)             
        {
            this.Node = violatingType;
            this.ViolatingField = violatingField;
        }
    }
}