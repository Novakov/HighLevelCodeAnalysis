﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Graphs;

namespace CodeModel.Primitives.Mutators
{
    public class AddAssemblies : IGraphMutator
    {
        private readonly Assembly[] assemblies;

        public AddAssemblies(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        public AddAssemblies(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies.ToArray();
        }

        public void Mutate(Graph model)
        {
            foreach (var assembly in assemblies)
            {
                model.AddNode(new AssemblyNode(assembly));
            }
        }
    }
}