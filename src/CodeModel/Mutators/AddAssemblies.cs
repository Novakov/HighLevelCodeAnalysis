using System.Reflection;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class AddAssemblies : IGraphMutator
    {
        private readonly Assembly[] assemblies;

        public AddAssemblies(params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
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