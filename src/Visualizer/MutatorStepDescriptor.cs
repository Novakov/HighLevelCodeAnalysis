using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Builder;

namespace Visualizer
{
    internal abstract class StepDescriptor
    {
        public IEnumerable<string> Provides { get; protected set; }
        public IEnumerable<string> Needs { get; set; }
        public IEnumerable<string> OptionalNeeds { get; protected set; }
    }

    internal class MutatorStepDescriptor : StepDescriptor
    {
        public Type Type { get; private set; }

        public MutatorStepDescriptor(CodeModelBuilder modelBuilder, Type type)
        {
            this.Type = type;

            this.Provides = modelBuilder.GetProvidedResources(type);

            this.Needs = modelBuilder.GetNeededResources(type);

            this.OptionalNeeds = modelBuilder.GetOptionalNeeds(type);          
        }

        public override string ToString()
        {
            return this.Type.Name;
        }        
    }

    internal class RequiredResources : StepDescriptor
    {
        public RequiredResources(IEnumerable<string> neededResources)
        {
            this.Needs = neededResources.ToList();
            this.Provides = Enumerable.Empty<string>();
            this.OptionalNeeds = Enumerable.Empty<string>();
        }

        public override string ToString()
        {
            return "Require: " + string.Join(", ", this.Needs);
        }
    }
}