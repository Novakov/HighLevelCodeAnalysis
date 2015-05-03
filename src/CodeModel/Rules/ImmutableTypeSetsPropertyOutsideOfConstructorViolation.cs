using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    [Violation(DisplayText = "{ViolatingMethod} sets property value")]
    public class ImmutableTypeSetsPropertyOutsideOfConstructorViolation : Violation, INodeViolation
    {
        public MethodNode ViolatingMethod { get; private set; }
        public Node Node { get; private set; }

        public ImmutableTypeSetsPropertyOutsideOfConstructorViolation(TypeNode violatingType, MethodNode violatingMethod)            
        {
            this.Node = violatingType;
            this.ViolatingMethod = violatingMethod;
        }
    }
}