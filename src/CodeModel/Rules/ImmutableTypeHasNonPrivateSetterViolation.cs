using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasNonPrivateSetterViolation : Violation, INodeViolation
    {
        public PropertyNode ViolatingProperty { get; private set; }
        public Node Node { get; private set; }

        public ImmutableTypeHasNonPrivateSetterViolation(TypeNode violatingType, PropertyNode violatingProperty)         
        {
            this.Node = violatingType;

            ViolatingProperty = violatingProperty;
        }
    }
}