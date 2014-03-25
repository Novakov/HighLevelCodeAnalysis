using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class AddApplicationEntryPoint : IGraphMutator
    {
        public void Mutate(Graph model)
        {
            model.AddNode(new ApplicationEntryPoint());
        }
    }
}