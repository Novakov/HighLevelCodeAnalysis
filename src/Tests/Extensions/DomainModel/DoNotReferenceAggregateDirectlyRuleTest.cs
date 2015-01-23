using System;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.DomainModel;
using CodeModel.Extensions.DomainModel.Mutators;
using CodeModel.Extensions.DomainModel.Rules;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Rules;
using TestTarget.DomainModel;

namespace Tests.Extensions.DomainModel
{
    [TestFixture]
    public class DoNotReferenceAggregateDirectlyRuleTest : BaseRuleTest<DoNotReferenceAggregateDirectlyRule>, IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        private AggregateNode OrganizationUnit
        {
            get { return this.Builder.Model.GetNodeForType<AggregateNode>(typeof (OrganizationUnit)); }
        }

        private AggregateNode OtherAggregate
        {
            get { return this.Builder.Model.GetNodeForType<AggregateNode>(typeof(OtherAggregate)); }
        }

        [Test]
        public void ViolationForOneAggregateReferencingAnotherByNavigationProperty()
        {
            // arrange
            BuildModel(typeof (OrganizationUnit), typeof (OtherAggregate));

            // act
            this.Verify(Builder);

            // assert            
            var orgUnitProperty = Builder.Model.GetNodeForProperty(Get.PropertyOf<OtherAggregate>(x => x.OrganizationUnit));
            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .InstanceOf<DirectAggregateReferenceViolation>()
                .And.Property("ReferencedAggregate").EqualTo(OrganizationUnit)
                .And.Property("Node").EqualTo(orgUnitProperty)
            );
        }

        [Test]
        public void ViolationForEntityReferencingAggregateByNavigationProperty()
        {
            // arrange
            BuildModel(typeof(OrganizationUnit), typeof(SomeOtherEntity));

            // act
            this.Verify(Builder);

            // assert
            var orgUnitProperty = Builder.Model.GetNodeForProperty(Get.PropertyOf<SomeOtherEntity>(x => x.OrganizationUnit));
            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .InstanceOf<DirectAggregateReferenceViolation>()
                .And.Property("ReferencedAggregate").EqualTo(OrganizationUnit)
                .And.Property("Node").EqualTo(orgUnitProperty)
            );
        }

        private void BuildModel(params Type[] types)
        {
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            foreach (var type in types)
            {
                Builder.Model.AddNode(new TypeNode(type));
            }

            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();
        }
    }

}