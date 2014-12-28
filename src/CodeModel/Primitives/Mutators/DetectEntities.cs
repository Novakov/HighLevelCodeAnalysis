using CodeModel.Builder;
using CodeModel.Conventions;

namespace CodeModel.Primitives.Mutators
{
    public class DetectEntities : INodeMutator<TypeNode>
    {
        private readonly IEntityConvention convention;

        public DetectEntities(IEntityConvention convention)
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
