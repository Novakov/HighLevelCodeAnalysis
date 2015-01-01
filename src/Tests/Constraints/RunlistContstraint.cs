using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests.Constraints
{
    public class RunlistConstraint : IResolveConstraint
    {
        private readonly ConstraintBuilder builder;

        public RunlistConstraint And
        {
            get
            {
                this.builder.Append(new AndOperator());
                return this;
            }
        }

        public RunlistConstraint Not
        {
            get
            {
                this.builder.Append(new NotOperator());
                return this;
            }
        }

        public RunlistConstraint()
        {
            this.builder = new ConstraintBuilder();
        }

        public RunlistConstraint After(object element, params object[] after)
        {
            this.builder.Append(new PropertyConstraint("Elements", new ElementAfterElementsConstraint(element, after)));

            return this;
        }

        public RunlistConstraint Contains(object element)
        {
            this.builder.Append(new PropertyConstraint("Elements", new ContainsConstraint(element)));

            return this;
        }

        public Constraint Resolve()
        {
            return this.builder.Resolve();
        }
    }

    public class ElementAfterElementsConstraint : Constraint
    {
        private readonly object element;
        private readonly object[] after;

        public ElementAfterElementsConstraint(object element, object[] after)
        {
            this.element = element;
            this.after = after;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;

            var elements = ((IEnumerable<object>) actual).ToList();

            var indexOf = elements.IndexOf(this.element);
            
            return after.All(x => elements.IndexOf(x) < indexOf);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("Expected element {0} to be after elements ", this.element);
            writer.WriteCollectionElements(after, 0, after.Length - 1);
        }        
    }
}
