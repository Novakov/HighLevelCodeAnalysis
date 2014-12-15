using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsPropertyOutsideOfConstructorViolation : Violation
    {
        public MethodNode ViolatingMethod { get; private set; }

        public ImmutableTypeSetsPropertyOutsideOfConstructorViolation(TypeNode violatingType, MethodNode violatingMethod)
            : base(violatingType)
        {
            ViolatingMethod = violatingMethod;
        }
    }
}