using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeHasWritableFieldViolation : Violation
    {
        public FieldNode ViolatingField { get; private set; }

        public ImmutableTypeHasWritableFieldViolation(TypeNode violatingType, FieldNode violatingField) 
            : base(violatingType)
        {
            ViolatingField = violatingField;
        }
    }
}