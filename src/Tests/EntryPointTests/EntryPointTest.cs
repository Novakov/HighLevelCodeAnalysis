using System.Linq;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Graph = Tests.Constraints.Graph;

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
                .Nodes<ApplicationEntryPoint>(exactly: 1));
        }

        [Test]
        public void ShouldLinkEntryPointToMatchingNodes()
        {
            // arrange
            this.Builder.RunMutator<AddApplicationEntryPoint>();
            var nodeA1 = this.Builder.Model.AddNode(new SampleNode("A1"));
            var nodeA2 = this.Builder.Model.AddNode(new SampleNode("A2"));
            var nodeB1 = this.Builder.Model.AddNode(new SampleNode("B1"));

            // act
            this.Builder.RunMutator(new LinkApplicationEntryPointTo<SampleNode>(node => nodeA1 == node || nodeA2 == node));

            // assert
            var entryPoint = this.Builder.Model.Nodes.OfType<ApplicationEntryPoint>().Single();

            Assert.That(this.Builder.Model, Graph.Has
                .Links<ApplicationEntryCall>(exactly:1, from:entryPoint, to:nodeA1)
                .Links<ApplicationEntryCall>(exactly:1, from:entryPoint, to:nodeA2)
                .Links<ApplicationEntryCall>(exactly:0, from:entryPoint, to:nodeB1)
                );
        }

        public class SampleNode : Node
        {
            public SampleNode(string id)
                : base(id)
            {

            }
        }
    }
}
