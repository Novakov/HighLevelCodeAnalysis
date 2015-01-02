using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    public class ImmutableTypeSetsFieldOutsideOfConstructorViolation : Violation, INodeViolation
    {
        public MethodNode ViolatingMethod { get; private set; }
        public Node Node { get; private set; }

        public ImmutableTypeSetsFieldOutsideOfConstructorViolation(TypeNode violatingType, MethodNode violatingMethod)            
        {
            this.Node = violatingType;
            this.ViolatingMethod = violatingMethod;
        }
    }
}