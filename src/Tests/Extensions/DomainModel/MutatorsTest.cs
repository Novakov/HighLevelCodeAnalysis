using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.DomainModel;
using CodeModel.Extensions.DomainModel.Mutators;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;
using TestTarget.DomainModel;

namespace Tests.Extensions.DomainModel
{
    [TestFixture]
    public class MutatorsTest : IHaveBuilder
    {
        private static readonly Assembly TargetAssembly = typeof(Marker).Assembly;

        public CodeModelBuilder Builder { get; private set; }

        public AggregateNode OrganizationUnitNode
        {
            get { return this.Builder.Model.GetNodeForType<AggregateNode>(typeof(OrganizationUnit)); }
        }

        public EntityNode PersonNode
        {
            get { return this.Builder.Model.GetNodeForType<EntityNode>(typeof(Person)); }
        }

        public EntityNode SomeEntityNode
        {
            get { return this.Builder.Model.GetNodeForType<EntityNode>(typeof(SomeEntity)); }
        }

        [SetUp]
        public void SetUp()
        {
            this.Builder = new CodeModelBuilder();

            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            Builder.RunMutator(new AddAssemblies(TargetAssembly));
        }

        [Test]
        public void ShouldRecognizeEntity()
        {
            // arrange                                   
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<DetectEntities>();

            // assert            
            Assert.That(Builder.Model, Graph.Has
                .NodeForType<Person>(Is.InstanceOf<EntityNode>()));
        }

        [Test]
        public void ShouldRecognizeAggregates()
        {
            // arrange                                   
            Builder.RunMutator<AddTypes>();

            // act            
            Builder.RunMutator<DetectAggregates>();

            // assert            
            Assert.That(Builder.Model, Graph.Has
                .NodeForType<OrganizationUnit>(Is.InstanceOf<AggregateNode>()));
        }

        [Test]
        public void ShouldLinkAggregateToEntityItContains()
        {
            // arrange
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();

            // act
            Builder.RunMutator<LinkContainedEntities>();

            // assert
            var manager = Builder.Model.GetNodeForProperty(Get.PropertyOf<OrganizationUnit>(x => x.Manager));

            Assert.That(Builder.Model, Graph.Has
                .Links<HasOneEntityLink>(exactly: 1, @from: OrganizationUnitNode, to: PersonNode, matches: x => x.Via == manager));
        }

        [Test]
        public void ShouldLinkAggregateToEntityCollectionItContains()
        {
            // arrange
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();

            // act
            Builder.RunMutator<LinkContainedEntities>();

            // assert
            var employees = Builder.Model.GetNodeForProperty(Get.PropertyOf<OrganizationUnit>(x => x.Employees));

            Assert.That(Builder.Model, Graph.Has
                .Links<HasManyEntityLink>(exactly: 1, @from: OrganizationUnitNode, to: PersonNode, matches: x => x.Via == employees));
        }

        [Test]
        public void ShouldLinkEntityToEntityItContains()
        {
            // arrange
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();

            // act
            Builder.RunMutator<LinkContainedEntities>();

            // assert
            var hasOne = Builder.Model.GetNodeForProperty(Get.PropertyOf<Person>(x => x.OneEntity));

            Assert.That(Builder.Model, Graph.Has
                .Links<HasOneEntityLink>(exactly: 1, @from: PersonNode, to: SomeEntityNode, matches: x => x.Via == hasOne));
        }

        [Test]
        public void ShouldLinkEntityToEntityCollectionItContains()
        {
            // arrange
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();

            // act
            Builder.RunMutator<LinkContainedEntities>();

            // assert
            var entitySet = Builder.Model.GetNodeForProperty(Get.PropertyOf<Person>(x => x.EntitySet));

            Assert.That(Builder.Model, Graph.Has
                .Links<HasManyEntityLink>(exactly: 1, @from: PersonNode, to: SomeEntityNode, matches: x => x.Via == entitySet));
        }

        [Test]
        public void ShouldLinkReferenceFromEntityToAggregate()
        {
            // arrange
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();

            // act
            Builder.RunMutator<LinkContainedEntities>();

            // assert
            var managedOrganizationUnit = this.Builder.Model.GetNodeForProperty(Get.PropertyOf<Person>(x => x.ManagedOrganizationUnit));

            Assert.That(this.Builder.Model, Graph.Has
                .Links<HasOneEntityLink>(exactly: 1, from: PersonNode, to: OrganizationUnitNode, matches: x => x.Via == managedOrganizationUnit));
        }
    }
}