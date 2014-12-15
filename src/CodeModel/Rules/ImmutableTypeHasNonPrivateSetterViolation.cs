using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasNonPrivateSetterViolation : Violation
    {
        public const string NonPrivatePropertySetter = "NonPrivatePropertySetter";

        public PropertyNode ViolatingProperty { get; private set; }

        public ImmutableTypeHasNonPrivateSetterViolation(TypeIsImmutable rule, TypeNode violatingType, PropertyNode violatingProperty)
            : base(rule, violatingType, NonPrivatePropertySetter, null)
        {
            ViolatingProperty = violatingProperty;
        }
    }
}