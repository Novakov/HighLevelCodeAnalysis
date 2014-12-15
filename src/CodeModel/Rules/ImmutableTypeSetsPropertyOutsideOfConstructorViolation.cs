using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsPropertyOutsideOfConstructorViolation : Violation
    {
        public const string SettingPropertyOutsideOfConstructor = "SetPropertyOutsideOfCtor";

        public MethodNode ViolatingMethod { get; private set; }

        public ImmutableTypeSetsPropertyOutsideOfConstructorViolation(TypeIsImmutable rule, TypeNode violatingType, MethodNode violatingMethod)
            : base(rule, violatingType, SettingPropertyOutsideOfConstructor, null)
        {
            ViolatingMethod = violatingMethod;
        }
    }
}