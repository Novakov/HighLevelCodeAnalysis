using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests.Constraints
{
    public class GraphConstraint : IResolveConstraint
    {
        private readonly List<Constraint> constraints;

        public GraphConstraint()
        {
            this.constraints = new List<Constraint>();
        }

        public GraphConstraint Nodes<TNode>(int? exactly = null, Expression<Func<TNode, bool>> matches = null)
            where TNode : Node
        {
            var builder = new ConstraintBuilder();

            if (exactly.HasValue)
            {
                builder.Append(new ExactCountOperator(exactly.Value));
            }
            else
            {
                builder.Append(new SomeOperator());
            }

            builder.Append(new InstanceOfTypeConstraint(typeof(TNode)));

            if (matches != null)
            {
                builder.Append(new ExpressionConstraint<TNode>(matches));
            }

            this.constraints.Add(new PropertyConstraint("Nodes", builder.Resolve()));

            return this;
        }

        public GraphConstraint Links<TLink>(int? exactly = null, Node from = null, Node to = null, Expression<Func<TLink, bool>> matches = null)
            where TLink : Link
        {
            var builder = new ConstraintBuilder();

            if (exactly.HasValue)
            {
                builder.Append(new ExactCountOperator(exactly.Value));
            }
            else
            {
                builder.Append(new SomeOperator());
            }

            builder.Append(new InstanceOfTypeConstraint(typeof(TLink)));

            if (from != null)
            {
                builder.Append(new PropertyConstraint("Source", new EqualConstraint(from)));
            }

            if (to != null)
            {
                builder.Append(new PropertyConstraint("Target", new EqualConstraint(to)));
            }

            if (matches != null)
            {
                builder.Append(new ExpressionConstraint<TLink>(matches));
            }

            this.constraints.Add(new FilteredCollectionPropertyConstraint("Links", x => x.OfType<TLink>(), builder.Resolve()));

            return this;
        }

        public GraphConstraint NodeForType<T>(IResolveConstraint constraint)
        {
            this.constraints.Add(new NodeForTypeConstraint(typeof(T), constraint));

            return this;
        }

        public Constraint Resolve()
        {
            var builder = new ConstraintBuilder();

            foreach (var constraint in this.constraints)
            {
                builder.Append(constraint);
            }

            return builder.Resolve();
        }
    }

    public class FilteredCollectionPropertyConstraint : Constraint
    {
        private readonly string property;
        private readonly Func<IEnumerable<object>, IEnumerable<object>> func;
        private readonly Constraint innerConstraint;


        public FilteredCollectionPropertyConstraint(string property, Func<IEnumerable<object>, IEnumerable<object>> func, IResolveConstraint innerConstraint)
        {
            this.property = property;
            this.func = func;
            this.innerConstraint = innerConstraint.Resolve();
        }

        public override bool Matches(object input)
        {
            var unfiltered = input.GetType().GetProperty(this.property).GetValue(input);

            var filtered = this.func((IEnumerable<object>) unfiltered);

            this.actual = filtered;

            return this.innerConstraint.Matches(filtered);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("Property {0} ", this.property);
            innerConstraint.WriteDescriptionTo(writer);
        }
    }
}
