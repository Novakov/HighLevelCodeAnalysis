using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;

namespace CodeModel.Links
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
