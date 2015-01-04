using System.Collections.Generic;
using TestTarget.EventSourcing;

namespace TestTarget.DomainModel
{
    public class Person : EntityBase
    {
        public SomeEntity OneEntity { get; private set; }
        public ISet<SomeEntity> EntitySet { get; private set; }

        public OrganizationUnit ManagedOrganizationUnit { get; private set; }
    }

    public class SomeEntity : EntityBase
    {
    }

    public class OrganizationUnit : EntityBase, IAggregate
    {
        public Person Manager { get; private set; }
        public IList<Person> Employees { get; private set; }
    }

    public interface IAggregate
    {
    }
}