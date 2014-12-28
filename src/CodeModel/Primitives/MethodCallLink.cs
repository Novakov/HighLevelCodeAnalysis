using System;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;

namespace CodeModel.Primitives
{
    public class MethodCallLink : Link
    {
        public Type[] GenericMethodArguments { get; private set; }

        public PotentialType[][] ActualParameterTypes { get; private set; }

        public MethodCallLink(Type[] genericMethodArguments, PotentialType[][] actualParameterTypes)
        {
            this.GenericMethodArguments = genericMethodArguments;
            this.ActualParameterTypes = actualParameterTypes;
        }
    }
}
