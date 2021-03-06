using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.RuleEngine;
using CodeModel.Rules;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class MethodCanLeadToExecutionOfMoreThanOneCommandViolation : Violation, INodeViolation
    {
        public List<Node> Path { get; private set; }
        public Node Node { get; private set; }

        public MethodCanLeadToExecutionOfMoreThanOneCommandViolation(Node node, List<Node> path)       
        {
            this.Node = node;
            this.Path = path;
        }
    }
}