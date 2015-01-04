using System.Reflection;
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
    }
}