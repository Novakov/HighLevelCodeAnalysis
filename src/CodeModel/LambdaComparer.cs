using System;
using System.Collections.Generic;

namespace CodeModel
{
    public class LambdaComparer<T, TProperty> : IComparer<T>        
    {
        private readonly Func<T, TProperty> property;
        private readonly Comparer<TProperty> comparer;

        public LambdaComparer(Func<T, TProperty> property)
        {
            this.property = property;
            this.comparer = Comparer<TProperty>.Default;
        }

        public int Compare(T x, T y)
        {
            return this.comparer.Compare(this.property(x), this.property(y));
        }
    }

    public static class LambdaComparer<T>
    {
        public static LambdaComparer<T, TProperty> For<TProperty>(Func<T, TProperty> property)
        {
            return new LambdaComparer<T, TProperty>(property);
        }
    }
}