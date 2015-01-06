using CodeModel.Builder;
using CodeModel.Conventions;
using CodeModel.Dependencies;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Mutators
{
    [Provide(DomainModelResources.Entities)]
    [Need(Resources.Types)]
    public class DetectEntities : INodeMutator<TypeNode>
    {
        private readonly IDomainModelConvention convention;

        public DetectEntities(IDomainModelConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (!(node is EntityNode) && this.convention.IsEntity(node))
            {
                context.ReplaceNode(node, new EntityNode(node.Type));
            }
        }
    }
}
