using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CodeModel
{
    public class EquatableImmutableDictionary<TKey, TValue> : IEquatable<EquatableImmutableDictionary<TKey, TValue>> 
        where TValue : IEquatable<TValue>
    {
        private readonly IImmutableDictionary<TKey, TValue> dictionary;

        public static readonly EquatableImmutableDictionary<TKey, TValue> Empty = new EquatableImmutableDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue>.Empty); 

        public EquatableImmutableDictionary(IImmutableDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public EquatableImmutableDictionary(ImmutableDictionary<TKey, TValue> dictionary)            
        {
            this.dictionary = dictionary;
        }

        public EquatableImmutableDictionary(IDictionary<TKey, TValue> initialValues)
            : this(initialValues.ToImmutableDictionary())
        {}

        public TValue this[TKey key]
        {
            get { return this.dictionary[key]; }
        }

        public EquatableImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value)
        {
            return new EquatableImmutableDictionary<TKey, TValue>(this.dictionary.SetItem(key, value));
        }

        public bool Equals(EquatableImmutableDictionary<TKey, TValue> other)
        {
            if (this.dictionary.Count != other.dictionary.Count)
            {
                return false;
            }

            return this.dictionary.All(t => t.Value.Equals(other[t.Key]));
        }

        public override int GetHashCode()
        {
            return this.dictionary.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as EquatableImmutableDictionary<TKey, TValue>;

            return other != null && this.Equals(other);
        }
    }
}