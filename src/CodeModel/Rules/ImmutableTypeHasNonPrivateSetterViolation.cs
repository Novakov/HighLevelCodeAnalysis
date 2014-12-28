using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasNonPrivateSetterViolation : Violation
    {
        public PropertyNode ViolatingProperty { get; private set; }

        public ImmutableTypeHasNonPrivateSetterViolation(TypeNode violatingType, PropertyNode violatingProperty)
            : base(violatingType)
        {
            ViolatingProperty = violatingProperty;
        }
    }
}