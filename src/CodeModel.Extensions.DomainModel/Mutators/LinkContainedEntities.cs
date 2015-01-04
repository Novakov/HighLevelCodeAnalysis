using System;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Mutators
{
    public class LinkContainedEntities : INodeMutator<AggregateNode>, INodeMutator<EntityNode>
    {
        public void Mutate(AggregateNode node, IMutateContext context)
        {
            LinkHasOne(node, context);

            LinkHasMany(node, context);
        }

        public void Mutate(EntityNode node, IMutateContext context)
        {
            if (node is AggregateNode)
            {
                return;
            }

            LinkHasOne(node, context);

            LinkHasMany(node, context);
        }

        private static void LinkHasMany(TypeNode node, IMutateContext context)
        {
            var properties = from p in node.Type.GetProperties()
                where p.PropertyType.IsEnumerable()
                let type = p.PropertyType.GetEnumerableElement()
                let typeNode = context.FindNode<EntityNode>(x => x.Type == type)
                let propertyNode = context.FindNode<PropertyNode>(x => x.Property == p)
                where typeNode != null && propertyNode != null
                select new Action(() => context.AddLink(node, typeNode, new HasManyEntityLink(propertyNode)));

            foreach (var property in properties)
            {
                property();
            }
        }

        private static void LinkHasOne(TypeNode node, IMutateContext context)
        {
            var properties = from p in node.Type.GetProperties()
                let type = p.PropertyType
                let typeNode = context.FindNode<EntityNode>(x => x.Type == type)
                let propertyNode = context.FindNode<PropertyNode>(x => x.Property == p)
                where typeNode != null && propertyNode != null
                select new Action(() => context.AddLink(node, typeNode, new HasOneEntityLink(propertyNode)));

            foreach (var property in properties)
            {
                property();
            }
        }
    }
}