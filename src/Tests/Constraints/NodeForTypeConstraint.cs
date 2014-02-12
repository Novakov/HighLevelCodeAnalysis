using System;
using CodeModel;
using NUnit.Framework.Constraints;

namespace Tests.Constraints
{
    public class NodeForTypeConstraint : Constraint
    {
        private readonly Type type;
        private readonly Constraint constraint;

        public NodeForTypeConstraint(Type type, IResolveConstraint constraint)
        {
            this.type = type;
            this.constraint = constraint.Resolve();
        }

        public override bool Matches(object actual)
        {
            var graph = (CodeModel.Graphs.Graph) actual;
            var node = graph.GetNodeForType(this.type);

            this.actual = node;

            return this.constraint.Matches(node);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            this.constraint.WriteDescriptionTo(writer);
        }
    }
}