using TestTarget.EventSourcing;

namespace TestTarget.DomainModel
{
    public class Person : EntityBase
    {
         
    }

    public class OrganizationUnit : EntityBase, IAggregate
    {
        
    }

    public interface IAggregate
    {
    }
}