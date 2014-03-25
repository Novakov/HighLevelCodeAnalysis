using CodeModel.Builder;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;

namespace Tests.EntryPointTests
{
    [TestFixture]
    public class EntryPointTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();           
        }

        [Test]
        public void ShouldAddApplicationEntryPoint()
        {
            // arrange

            // act
            this.Builder.RunMutator<AddApplicationEntryPoint>();

            // assert
            Assert.That(this.Builder.Model, Graph.Has
                .Nodes<ApplicationEntryPoint>(exactly:1));
        }
    }
}
