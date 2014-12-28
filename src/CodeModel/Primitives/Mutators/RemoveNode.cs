using System;
using CodeModel.Builder;
using CodeModel.Graphs;

namespace CodeModel.Primitives.Mutators
{
    public class RemoveNode<TNode> : INodeMutator<TNode> 
        where TNode : Node
    {
        private readonly Func<TNode, bool> predicate;

        public RemoveNode(Func<TNode, bool> predicate)
        {
            this.predicate = predicate;
        }

        public void Mutate(TNode node, IMutateContext context)
        {
            if (predicate(node))
            {
                context.RemoveNode(node);
            }
        }
    }
}
