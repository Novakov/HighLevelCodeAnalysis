using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsFieldOutsideOfConstructorViolation : Violation
    {
        public MethodNode ViolatingMethod { get; private set; }

        public ImmutableTypeSetsFieldOutsideOfConstructorViolation(TypeNode violatingType, MethodNode violatingMethod)
            : base(violatingType)
        {
            ViolatingMethod = violatingMethod;
        }
    }
}