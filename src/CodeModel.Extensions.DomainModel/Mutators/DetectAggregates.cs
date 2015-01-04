using CodeModel.Builder;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Mutators
{
    public class DetectAggregates : INodeMutator<TypeNode>
    {
        private readonly IDomainModelConvention convention;

        public DetectAggregates(IDomainModelConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (node is AggregateNode)
            {
                return;
            }

            if (this.convention.IsAggregate(node))
            {
                context.ReplaceNode(node, new AggregateNode(node.Type));
            }
        }
    }
}