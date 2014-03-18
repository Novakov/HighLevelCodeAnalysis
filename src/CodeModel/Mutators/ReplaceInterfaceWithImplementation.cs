using System.CodeDom;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Convetions;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class ReplaceInterfaceWithImplementation : ICompositeMutator
    {
        public void Mutate(CodeModelBuilder bulder)
        {
            bulder.RunMutator<ReplaceMethodNodes>();
            bulder.RunMutator<ReplaceTypeNodes>();
        }

        private class ReplaceMethodNodes : INodeMutator<MethodNode>
        {
            private readonly IImplementingConvention convention;

            public ReplaceMethodNodes(IImplementingConvention convention)
            {
                this.convention = convention;
            }

            public void Mutate(MethodNode node, IMutateContext context)
            {
                var containedIn = (TypeNode)node.GetContainer();

                var implementedInterfaces = containedIn.OutboundLinks.OfType<ImplementsLink>();

                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (!this.convention.ShouldInlineImplementationsFor((TypeNode) implementedInterface.Target))
                    {
                        continue;
                    }

                    var interfaceMethod = node.Method.GetImplementedMethod(((TypeNode)implementedInterface.Target).Type);

                    if (interfaceMethod != null)
                    {
                        var interfaceMethodNode = context.LookupNode<MethodNode>(MethodNode.IdFor(interfaceMethod));

                        if (interfaceMethodNode != null)
                        {
                            context.ReplaceNode(interfaceMethodNode, node);
                        }
                    }
                }
            }
        }

        private class ReplaceTypeNodes : INodeMutator<TypeNode>
        {
            private readonly IImplementingConvention convention;

            public ReplaceTypeNodes(IImplementingConvention convention)
            {
                this.convention = convention;
            }

            public void Mutate(TypeNode node, IMutateContext context)
            {
                var implements = node.OutboundLinks.OfType<ImplementsLink>().ToList();

                foreach (var implement in implements)
                {
                    if (!this.convention.ShouldInlineImplementationsFor((TypeNode)implement.Target))
                    {
                        continue;
                    }

                    var target = implement.Target;
                    context.RemoveLink(implement);
                    context.ReplaceNode(target, node);
                }
            }
        }
    }
}