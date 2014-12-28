using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Graphs;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.EntryPoint)]
    public class AddApplicationEntryPoint : IGraphMutator
    {
        public void Mutate(Graph model)
        {
            model.AddNode(new ApplicationEntryPoint());
        }
    }
}