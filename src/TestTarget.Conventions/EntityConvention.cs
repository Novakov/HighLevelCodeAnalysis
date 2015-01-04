using CodeModel.Conventions;
using CodeModel.Extensions.DomainModel.Conventions;
using CodeModel.Primitives;
using TestTarget.EventSourcing;

namespace TestTarget.Conventions
{
    public class EntityConvention : IEntityConvention
    {
        public bool IsEntity(TypeNode node)
        {
            return typeof (EntityBase).IsAssignableFrom(node.Type);
        }
    }
}
