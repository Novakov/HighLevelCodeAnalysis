using System;
using System.Linq.Expressions;
using NUnit.Framework.Constraints;

namespace Tests.Constraints
{
    public class ExpressionConstraint<T> : Constraint
    {
        private readonly Expression<Func<T, bool>> predicate;
        private readonly Lazy<Func<T, bool>> compiledPredicate;

        public ExpressionConstraint(Expression<Func<T, bool>>  predicate)
        {
            this.predicate = predicate;
            this.compiledPredicate = new Lazy<Func<T, bool>>(() => this.predicate.Compile());
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;

            if (!(actual is T))
                return false;

            return this.compiledPredicate.Value((T)actual);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("value matching");
            writer.Write(this.predicate.ToString());
        }
    }
}