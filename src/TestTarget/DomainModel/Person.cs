using System;
using System.Collections.Generic;
using TestTarget.EventSourcing;

namespace TestTarget.DomainModel
{
    public class Person : EntityBase
    {
        public SomeEntity OneEntity { get; private set; }
        public ISet<SomeEntity> EntitySet { get; private set; }

        public OrganizationUnit ManagedOrganizationUnit { get; private set; }

        public void DoSomething()
        {
            
        }

        public void CallSomething()
        {
            this.DoSomething();
        }

        public void CallSomethingOnSomeEntity()
        {
            this.OneEntity.DoSomethingOnSomeEntity();
        }
    }

    public class SomeEntity : EntityBase
    {
        public void DoSomethingOnSomeEntity()
        {
            
        }
    }

    public class OrganizationUnit : EntityBase, IAggregate
    {
        public Person Manager { get; private set; }
        public IList<Person> Employees { get; private set; }

        [Reference(typeof(OrganizationUnit))]
        public int ParentOrganizationUnit { get; private set; }

        public void LetManagerKnow()
        {
            this.Manager.DoSomething();
        }      
    }

    public class ThirdAggregate : EntityBase, IAggregate
    {
        public void DoSomethingOnEntityInAggregate(OtherAggregate other)
        {
            other.OtherEntity.DoSomething();
        }
    }

    public class OtherEntity : EntityBase
    {
        [Reference(typeof(OrganizationUnit))]
        public int AffectedOrganizationUnitId { get; set; }
    }

    public class OtherAggregate : EntityBase, IAggregate
    {
        public SomeOtherEntity OtherEntity { get; private set; }

        public OrganizationUnit OrganizationUnit { get; private set; }
    }

    public class SomeOtherEntity : EntityBase
    {
        public OrganizationUnit OrganizationUnit { get; private set; }

        public void DoSomething()
        {
        }
    }

    public class ReferenceAttribute : Attribute
    {
        public Type TargetAggregate { get; private set; }

        public ReferenceAttribute(Type targetAggregate)
        {
            TargetAggregate = targetAggregate;
        }
    }

    public interface IAggregate
    {
    }
}