using System;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class LinkToContainer : INodeMutator<MethodNode>, INodeMutator<FieldNode>, INodeMutator<PropertyNode>, INodeMutator<TypeNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            LinkIfPossible<TypeNode>(node, context, x => x.Type == node.Method.DeclaringType);
        }

        public void Mutate(FieldNode node, IMutateContext context)
        {
            LinkIfPossible<TypeNode>(node, context, x => x.Type == node.Field.DeclaringType);
        }

        public void Mutate(PropertyNode node, IMutateContext context)
        {
            LinkIfPossible<TypeNode>(node, context, x => x.Type == node.Property.DeclaringType);
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            LinkIfPossible<AssemblyNode>(node, context, x => x.Assembly == node.Type.Assembly);
        }

        private static void LinkIfPossible<TNode>(Node source, IMutateContext context, Func<TNode, bool> targetPredicate)
            where TNode : Node
        {
            var typeNode = context.FindNodes(targetPredicate).SingleOrDefault();
            if (typeNode != null)
            {
                context.AddLink(source, typeNode, new ContainedInLink());
            }
        }
    }
}