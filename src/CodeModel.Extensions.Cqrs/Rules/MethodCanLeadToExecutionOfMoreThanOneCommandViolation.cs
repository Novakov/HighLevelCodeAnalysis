using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodCanLeadToExecutionOfMoreThanOneCommandViolation : Violation
    {
        public const string ViolationCategory = "OnlyOneCommandExecutionOnPathRule";

        public List<Node> Path { get; private set; }

        public MethodCanLeadToExecutionOfMoreThanOneCommandViolation(OnlyOneCommandExecutionOnPathRule rule, Node node, List<Node> path)
            : base(rule, node, ViolationCategory, null)
        {
            Path = path;
        }
    }
}