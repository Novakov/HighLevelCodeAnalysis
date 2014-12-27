using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel.Mutators
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