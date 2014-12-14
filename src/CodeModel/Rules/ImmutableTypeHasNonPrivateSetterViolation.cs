using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasNonPrivateSetterViolation : Violation
    {
        public const string NonPrivatePropertySetter = "NonPrivatePropertySetter";

        public ImmutableTypeHasNonPrivateSetterViolation(TypeIsImmutable rule, PropertyNode propertyNode)
            : base(rule, propertyNode, NonPrivatePropertySetter, null)
        {
            
        }
    }
}