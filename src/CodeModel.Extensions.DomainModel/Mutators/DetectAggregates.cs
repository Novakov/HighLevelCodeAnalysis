using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Mutators
{
    [Provide(DomainModelResources.Aggregates)]
    [Need(Resources.Types)]
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