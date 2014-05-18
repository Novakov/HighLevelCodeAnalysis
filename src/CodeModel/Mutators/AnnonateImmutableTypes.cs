using CodeModel.Annotations;
using CodeModel.Builder;
using CodeModel.Convetions;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class AnnonateImmutableTypes : INodeMutator<TypeNode>
    {
        private readonly IImmutablityConvention convention;

        public AnnonateImmutableTypes(IImmutablityConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (convention.IsImmutableType(node))
            {
                node.Annonate(new Immutable());
            }
        }
    }
}