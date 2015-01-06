using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.DomainModel;
using CodeModel.Extensions.DomainModel.Mutators;
using CodeModel.Extensions.DomainModel.Rules;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Rules;
using TestTarget.Conventions;
using TestTarget.DomainModel;

namespace Tests.Extensions.DomainModel
{
    [TestFixture]
    public class AvoidBidirectionalReferenceRuleTest : BaseRuleTest<AvoidBidirectionalReferenceRule>
    {
        [Test]
        public void ShouldReportViolationForBidirectionalReferenceBetweenEntityAndAggregate()
        {
            // arrange
            var model = new CodeModelBuilder();

            model.RegisterConventionsFrom(Marker.Assembly);

            model.Model.AddNode(new TypeNode(typeof (OrganizationUnit)));
            model.Model.AddNode(new TypeNode(typeof (Person)));

            model.RunMutator<AddProperties>();
            model.RunMutator<DetectEntities>();
            model.RunMutator<DetectAggregates>();
            model.RunMutator<LinkContainedEntities>();

            // act
            this.Verify(model);            

            // assert
            var organizationUnit = model.Model.GetNodeForType<AggregateNode>(typeof(OrganizationUnit));
            var person = model.Model.GetNodeForType<EntityNode>(typeof(Person));

            Assert.That(this.VerificationContext.Violations, Has
                .Some
                .InstanceOf<BidirectionalReferenceViolation>()
                .And.Property("Side1").EqualTo(organizationUnit)
                .And.Property("Side2").EqualTo(person)
            );
        }
    }
}