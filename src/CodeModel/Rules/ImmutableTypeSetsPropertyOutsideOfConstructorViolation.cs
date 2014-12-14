using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsPropertyOutsideOfConstructorViolation : Violation
    {
        public const string SettingPropertyOutsideOfConstructor = "SetPropertyOutsideOfCtor";

        public ImmutableTypeSetsPropertyOutsideOfConstructorViolation(TypeIsImmutable rule, MethodNode method)
            : base(rule, method, SettingPropertyOutsideOfConstructor, null)
        {
            
        }
    }
}