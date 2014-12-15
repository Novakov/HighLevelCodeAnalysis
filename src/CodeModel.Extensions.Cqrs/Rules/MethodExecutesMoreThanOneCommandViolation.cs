using CodeModel.Graphs;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodExecutesMoreThanOneCommandViolation : Violation
    {
        public MethodExecutesMoreThanOneCommandViolation(Node node) 
            : base(node)
        {
            
        }
    }
}