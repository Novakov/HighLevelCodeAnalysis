using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodCanLeadToExecutionOfMoreThanOneCommandViolation : Violation
    {
        public List<Node> Path { get; private set; }

        public MethodCanLeadToExecutionOfMoreThanOneCommandViolation(Node node, List<Node> path)
            : base(node)
        {
            Path = path;
        }
    }
}