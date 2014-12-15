using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsFieldOutsideOfConstructorViolation : Violation
    {
        public const string SettingFieldOutsideOfConstructor = "SetFieldOutsideOfCtor";

        public MethodNode ViolatingMethod { get; private set; }

        public ImmutableTypeSetsFieldOutsideOfConstructorViolation(TypeIsImmutable rule, TypeNode violatingType, MethodNode violatingMethod)
            : base(rule, violatingType, SettingFieldOutsideOfConstructor, null)
        {
            ViolatingMethod = violatingMethod;
        }
    }
}