using System;
using System.Collections.Generic;

namespace CodeModel.FlowAnalysis
{
    public class TypeAnalysisState : IEquatable<TypeAnalysisState>
    {
        private readonly EquatableImmutableStack<PotentialType> stack;
        private readonly EquatableImmutableDictionary<int, PotentialType> variables;
        private readonly EquatableImmutableDictionary<int, PotentialType> parameters;

        public readonly static TypeAnalysisState Empty = new TypeAnalysisState(new Dictionary<int, PotentialType>(), new Dictionary<int, PotentialType>());

        public TypeAnalysisState(EquatableImmutableStack<PotentialType> stack, EquatableImmutableDictionary<int, PotentialType> variables, EquatableImmutableDictionary<int, PotentialType> parameters)
        {
            this.stack = stack;
            this.variables = variables;
            this.parameters = parameters;
        }

        public TypeAnalysisState(IDictionary<int, PotentialType> variables, IDictionary<int, PotentialType> parameters)
            : this(EquatableImmutableStack<PotentialType>.Empty, new EquatableImmutableDictionary<int, PotentialType>(variables), new EquatableImmutableDictionary<int, PotentialType>(parameters))
        {
        }

        public bool Equals(TypeAnalysisState other)
        {
            return this.stack.Equals(other.stack)
                   && this.variables.Equals(other.variables)
                   && this.parameters.Equals(other.parameters);
        }

        public override bool Equals(object obj)
        {
            var other = obj as TypeAnalysisState;

            return other != null && this.Equals(other);
        }

        public override int GetHashCode()
        {            
            return 0;
        }

        public TypeAnalysisState Drop(int count)
        {
            return this.Mutate(newStack: this.stack.Drop(count));
        }

        public TypeAnalysisState Pop(Action<PotentialType> action)
        {
            return this.Mutate(newStack: this.stack.Pop(action));
        }

        public TypeAnalysisState Pop(out PotentialType value)
        {
            return this.Mutate(newStack: this.stack.Pop(out value));
        }

        public TypeAnalysisState PopMany(int count, out PotentialType[] values)
        {
            return this.Mutate(newStack: this.stack.PopMany(count, out values));
        }

        public TypeAnalysisState Push(PotentialType value)
        {
            return this.Mutate(newStack: this.stack.Push(value));
        }

        public PotentialType Variable(int index)
        {
            return this.variables[index];
        }

        public TypeAnalysisState WithVariable(int index, PotentialType type)
        {
            return this.Mutate(newVariables: this.variables.SetItem(index, type));
        }

        public PotentialType Param(int index)
        {
            return this.parameters[index];
        }

        public TypeAnalysisState WithParam(int index, PotentialType type)
        {
            return this.Mutate(newParams: this.parameters.SetItem(index, type));
        }

        private TypeAnalysisState Mutate(EquatableImmutableStack<PotentialType> newStack = null, EquatableImmutableDictionary<int, PotentialType> newVariables = null, EquatableImmutableDictionary<int, PotentialType> newParams = null)
        {
            return new TypeAnalysisState(newStack ?? this.stack, newVariables ?? this.variables, newParams ?? this.parameters);
        }
    }
}