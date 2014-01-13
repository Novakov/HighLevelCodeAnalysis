using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel.Builder
{
    public interface IMutateContext
    {
        TNode AddNode<TNode>(TNode node)
            where TNode : Node;

        void RemoveNode(Node node);

        void ReplaceNode(Node oldNode, Node newNode);
    }
}