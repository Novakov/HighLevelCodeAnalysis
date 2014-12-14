using CodeModel.Graphs;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodExecutesMoreThanOneCommandViolation : Violation
    {
        public const string MethodExecutesMoreThanOneCommandViolationCategory = "InvokeOnlyOneCommand";

        public MethodExecutesMoreThanOneCommandViolation(InvokeOnlyOneCommandInMethod rule, Node node) 
            : base(rule, node, MethodExecutesMoreThanOneCommandViolationCategory, null)
        {
            
        }
    }
}