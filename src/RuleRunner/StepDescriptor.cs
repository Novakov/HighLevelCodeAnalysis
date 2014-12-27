using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Rules;

namespace RuleRunner
{
    public class StepDescriptor
    {
        public Type Type { get; private set; }

        public IEnumerable<string> Provides { get; private set; }

        public IEnumerable<string> Needs { get; set; }

        public bool IsRule { get; private set; }

        public bool IsMutator { get; private set; }

        public StepDescriptor(CodeModelBuilder modelBuilder, Type type)
        {
            this.Type = type;

            this.Provides = modelBuilder.GetProvidedResources(type);

            this.Needs = modelBuilder.GetNeededResources(type);

            this.IsRule = typeof(IRule).IsAssignableFrom(this.Type);
            this.IsMutator = typeof(IMutator).IsAssignableFrom(this.Type);
        }

        public override string ToString()
        {
            return this.Type.Name;
        }
    }
}