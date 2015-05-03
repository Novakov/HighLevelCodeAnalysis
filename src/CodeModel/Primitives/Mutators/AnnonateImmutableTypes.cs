using CodeModel.Annotations;
using CodeModel.Builder;
using CodeModel.Conventions;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.ImmutableAnnotation)]
    [Need(Resources.Types)]
    public class AnnonateImmutableTypes : INodeMutator<TypeNode>
    {
        private readonly IImmutablityConvention convention;

        public AnnonateImmutableTypes(IImmutablityConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (this.convention.IsImmutableType(node))
            {
                node.Annonate(new Immutable());
            }
        }
    }
}