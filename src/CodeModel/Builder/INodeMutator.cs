using CodeModel.Graphs;
namespace CodeModel.Builder
{
    public interface INodeMutator<in TNode> : IMutator, INodeMutator
        where TNode : Node
    {
        void Mutate(TNode node, IMutateContext context);
    }

    public interface INodeMutator
    {
         
    }
}