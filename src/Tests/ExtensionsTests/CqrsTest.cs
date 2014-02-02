using System.Reflection;
using CodeModel.Builder;
using CodeModel.Extensions.Cqrs;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;
using TestTarget.Cqrs;

namespace Tests.ExtensionsTests
{
    public class CqrsTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void ShouldRecognizeQueryTypes()
        {            
            // arrange
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();            

            // act
            Builder.RunMutator<DetectQueries>();

            // assert

            Assert.That(Builder.Model, Graph.Has
                .NodeForType<GetUser>(Is.InstanceOf<QueryNode>()));
        }
    }
}
