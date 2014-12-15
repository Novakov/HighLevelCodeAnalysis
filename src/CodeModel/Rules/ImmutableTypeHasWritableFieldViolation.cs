using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasWritableFieldViolation : Violation
    {
        public const string WritableField = "WritableField";

        public FieldNode ViolatingField { get; private set; }

        public ImmutableTypeHasWritableFieldViolation(TypeIsImmutable rule, TypeNode violatingType, FieldNode violatingField) 
            : base(rule, violatingType, WritableField, null)
        {
            ViolatingField = violatingField;
        }
    }
}