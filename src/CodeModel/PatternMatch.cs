using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel
{
    public class PatternMatch
    {
        public static PatternMatch<TInput, TResult> For<TInput, TResult>()
        {
            return new PatternMatch<TInput, TResult>();
        }
    }

    public class PatternMatch<TInput, TResult>
    {
        private Func<TInput, TResult> @default;

        private readonly List<Tuple<Func<TInput, bool>, Func<TInput, TResult>>> clauses;

        public PatternMatch()
        {
            this.clauses = new List<Tuple<Func<TInput, bool>, Func<TInput, TResult>>>();
        }

        public PatternMatch<TInput, TResult> Default(Func<TInput, TResult> defaultValue)
        {
            this.@default = defaultValue;

            return this;
        }

        public PatternMatchClause<TInput> When(Func<TInput, bool> clauseCondition)
        {
            return new PatternMatchClause<TInput>(this, clauseCondition);
        }

        public PatternMatchClause<TSubType> When<TSubType>()
            where TSubType : TInput
        {
            Func<TInput, bool> clauseCondition = input => input != null && typeof(TSubType).IsAssignableFrom(input.GetType());

            return new PatternMatchClause<TSubType>(this, clauseCondition);
        }

        public PatternMatchClause<TSubType> When<TSubType>(Func<TSubType, bool> clauseCondition)
            where TSubType : TInput
        {
            return new PatternMatchClause<TSubType>(this, input =>
            {
                if (input != null && typeof(TSubType).IsAssignableFrom(input.GetType()))
                {
                    return clauseCondition((TSubType)input);
                }

                return false;
            });
        }

        public Func<TInput, TResult> AsDelegate()
        {
            return this.Match;
        }

        public TResult Match(TInput value)
        {
            var firstMatching = this.clauses.FirstOrDefault(x => x.Item1(value));

            if (firstMatching != null)
            {
                return firstMatching.Item2(value);
            }

            if (this.@default != null)
            {
                return this.@default(value);
            }

            throw new InvalidOperationException("No clause matches input value");
        }

        public class PatternMatchClause<TMiddle>
            where TMiddle : TInput
        {
            private readonly PatternMatch<TInput, TResult> parent;
            private readonly Func<TInput, bool> clauseCondition;

            public PatternMatchClause(PatternMatch<TInput, TResult> parent, Func<TInput, bool> clauseCondition)
            {
                this.parent = parent;
                this.clauseCondition = clauseCondition;
            }

            public PatternMatch<TInput, TResult> Return(Func<TMiddle, TResult> value)
            {
                Func<TInput, TResult> f = input => value((TMiddle)input);

                this.parent.clauses.Add(Tuple.Create(this.clauseCondition, f));

                return this.parent;
            }
        }
    }
}