using CodeModel.Convetions;
using CodeModel.Model;
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
