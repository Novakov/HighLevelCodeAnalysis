using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasWritableFieldViolation : Violation
    {
        public const string WritableField = "WritableField";

        public ImmutableTypeHasWritableFieldViolation(TypeIsImmutable rule, FieldNode node) 
            : base(rule, node, WritableField, null)
        {
        }
    }
}