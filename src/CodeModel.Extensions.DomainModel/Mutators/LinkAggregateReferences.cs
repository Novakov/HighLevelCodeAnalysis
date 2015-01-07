using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Mutators
{
    [Need(DomainModelResources.Aggregates, DomainModelResources.Entities)]
    public class LinkAggregateReferences : INodeMutator<EntityNode>
    {
        private readonly IDomainModelConvention convention;

        public LinkAggregateReferences(IDomainModelConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(EntityNode node, IMutateContext context)
        {           
            foreach (var property in node.Type.GetProperties())
            {
                LinkProperty(node, context, property);
            }
        }

        private void LinkProperty(Node node, IMutateContext context, PropertyInfo property)
        {
            if (this.convention.IsAggregateReference(property))
            {
                var targetAggregate = this.convention.GetReferenceAggregateType(property);

                var aggregateNode = context.FindNode<AggregateNode>(x => x.Type == targetAggregate);

                var propertyNode = context.FindNode<PropertyNode>(x => x.Property == property);

                if (aggregateNode != null)
                {
                    context.AddLink(node, aggregateNode, new AggregateReferenceLink(propertyNode));
                }
            }
        }
    }
}