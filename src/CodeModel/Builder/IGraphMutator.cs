using CodeModel.Graphs;

namespace CodeModel.Builder
{
    public interface IGraphMutator : IMutator
    {
        void Mutate(Graph model);
    }
}