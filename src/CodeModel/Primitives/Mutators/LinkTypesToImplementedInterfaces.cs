using System.Linq;
using CodeModel.Builder;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.ImplementsLink)]
    [Need(Resources.Types)]
    public class LinkTypesToImplementedInterfaces : INodeMutator<TypeNode>
    {
        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var @interface in node.Type.GetInterfaces())
            {
                var interfaceNode = context.FindNodes<TypeNode>(x => x.Type == @interface).FirstOrDefault();

                if (interfaceNode != null)
                {
                    context.AddLink(node, interfaceNode, new ImplementsLink());
                }
            }
        }
    }
}