using CodeModel.Model;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsFieldOutsideOfConstructorViolation : Violation
    {
        public const string SettingFieldOutsideOfConstructor = "SetFieldOutsideOfCtor";

        public ImmutableTypeSetsFieldOutsideOfConstructorViolation(TypeIsImmutable rule, MethodNode method)
            : base(rule, method, SettingFieldOutsideOfConstructor, null)
        {
            
        }
    }
}