using System;
using System.Reflection;
using CodeModel.Conventions;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Primitives;
using TestTarget.DomainModel;
using TestTarget.EventSourcing;

namespace TestTarget.Conventions
{
    public class DomainModelConvention : IDomainModelConvention
    {
        public bool IsEntity(TypeNode node)
        {
            return typeof(EntityBase).IsAssignableFrom(node.Type) && !typeof(IAggregate).IsAssignableFrom(node.Type);
        }

        public bool IsAggregate(TypeNode node)
        {
            return typeof(EntityBase).IsAssignableFrom(node.Type) && typeof(IAggregate).IsAssignableFrom(node.Type);
        }

        public bool IsAggregateReference(PropertyInfo property)
        {
            return property.GetCustomAttribute<ReferenceAttribute>() != null;
        }

        public Type GetReferenceAggregateType(PropertyInfo property)
        {
            return property.GetCustomAttribute<ReferenceAttribute>().TargetAggregate;
        }
    }
}
