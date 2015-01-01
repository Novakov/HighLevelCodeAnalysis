using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace CodeModel
{
    [DebuggerDisplay("Count = {stack.Count}")]
    public class EquatableImmutableStack<T> : IEnumerable<T>, IEquatable<EquatableImmutableStack<T>>
        where T : IEquatable<T>
    {
        public static readonly EquatableImmutableStack<T> Empty = new EquatableImmutableStack<T>();

        private readonly IImmutableList<T> stack;

        public T Top { get { return this.stack[this.stack.Count - 1]; } }

        private EquatableImmutableStack()
            : this(ImmutableList<T>.Empty)
        {
        }

        private EquatableImmutableStack(IImmutableList<T> list)
        {
            this.stack = list;
        }

        public bool Equals(EquatableImmutableStack<T> other)
        {
            if (this.stack.Count != other.stack.Count)
            {
                return false;
            }

            for (int i = 0; i < this.stack.Count; i++)
            {
                if (!this.stack[i].Equals(other.stack[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.stack.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as EquatableImmutableStack<T>;

            return other != null && this.Equals(other);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public EquatableImmutableStack<T> Push(T value)
        {
            return new EquatableImmutableStack<T>(this.stack.Add(value));
        }

        public EquatableImmutableStack<T> Pop(out T value)
        {
            value = this.stack[this.stack.Count - 1];

            return new EquatableImmutableStack<T>(this.stack.RemoveAt(this.stack.Count - 1));
        }

        public EquatableImmutableStack<T> PopMany(int count, out T[] value)
        {
            value = new T[count];

            for (int i = 0; i < count; i++)
            {
                value[i] = this.stack[this.stack.Count - count + i];
            }

            return new EquatableImmutableStack<T>(this.stack.RemoveRange(this.stack.Count - count, count));
        }

        public EquatableImmutableStack<T> Pop(Action<T> withValue)
        {
            T value;
            var updated = this.Pop(out value);

            withValue(value);

            return updated;
        }

        public EquatableImmutableStack<T> Drop(int count)
        {
            return new EquatableImmutableStack<T>(this.stack.RemoveRange(this.stack.Count - count, count));
        }
    }
}