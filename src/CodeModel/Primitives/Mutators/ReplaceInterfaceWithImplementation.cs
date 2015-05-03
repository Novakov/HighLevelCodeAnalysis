using System.Linq;
using CodeModel.Builder;
using CodeModel.Conventions;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.InlinedImplementations)]
    [Need(Resources.ImplementsLink, Resources.LinkToContainer)]
    [OptionalNeed(Resources.Types, Resources.LinkToContainer, Resources.Methods, Resources.MethodCallLinks, Resources.Dependencies)]
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

                if (containedIn.Type.IsInterface)
                {
                    return;
                }

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