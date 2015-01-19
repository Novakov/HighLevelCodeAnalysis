using System;

namespace TestTarget.DomainModel
{
    public class CallEntityMethods
    {
        private readonly Func<OrganizationUnit> repository;

        public CallEntityMethods(Func<OrganizationUnit> repository)
        {
            this.repository = repository;
        }

        public void CallEntityMethod()
        {
            var organizationUnit = this.repository();

            organizationUnit.Manager.DoSomething();
        }

        public void CallAggregateMethod()
        {
            var organizationUnit = this.repository();

            organizationUnit.LetManagerKnow();
        }
    }
}