using CodeModel.Graphs;

namespace CodeModel.Builder
{
    public interface IMutateContext
    {
        TNode AddNode<TNode>(TNode node)
            where TNode : Node;

        void RemoveNode(Node node);
    }
}