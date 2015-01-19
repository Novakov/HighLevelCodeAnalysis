using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    [Violation(DisplayText = "{Node} calls entity method {CalledMethod}")]
    public class DoNotCallEntityMethodsFromOutsideOfAggregateViolation : Violation, INodeViolation
    {
        public Node Node { get; private set; }

        public MethodNode CalledMethod { get; private set; }

        public DoNotCallEntityMethodsFromOutsideOfAggregateViolation(MethodNode callingNode, MethodNode calledMethod)
        {
            this.Node = callingNode;
            this.CalledMethod = calledMethod;
        }
    }
}