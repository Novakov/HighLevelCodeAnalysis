using System.Linq;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class GraphViewTest
    {
        [Test]
        public void ShouldFilterOutNodes()
        {
            // arrange
            var graph = new Graph();

            var nodeA1 = graph.AddNode(new SampleNode("A1"));
            var nodeA2 = graph.AddNode(new SampleNode("A2"));

            var nodeB1 = graph.AddNode(new SampleNode("B1"));
            var nodeB2 = graph.AddNode(new SampleNode("B2"));

            // act
            var view = graph.PrepareView(x => x.Id.StartsWith("A"), x => true);


            // assert
            Assert.That(view.Nodes.Select(x => x.Node), Has
                .Member(nodeA1)
                .And.Member(nodeA2)
                .And.Not.Member(nodeB1)
                .And.Not.Member(nodeB2),
                "Nodes has not been filtered");
        }

        [Test]
        public void ShouldFilterOutLinks()
        {
            // arrange
            var graph = new Graph();

            var nodeA1 = graph.AddNode(new SampleNode("A1"));
            var nodeA2 = graph.AddNode(new SampleNode("A2"));

            var link1 = graph.AddLink(nodeA1, nodeA2, new SampleLink("1"));
            var link2 = graph.AddLink(nodeA1, nodeA2, new SampleLink("2"));

            // act
            var view = graph.PrepareView(x => true, x => (x is SampleLink) && ((SampleLink)x).Name == "1");


            // assert
            Assert.That(view.Links, Has
                .Member(link1)
                .And.Not.Member(link2),
                "Nodes has not been filtered");
        }

        [Test]
        public void NodeViewShouldNotIncludeLinksNotMatchingPredicate()
        {
            // arrange
            var graph = new Graph();

            var nodeA1 = graph.AddNode(new SampleNode("A1"));
            var nodeA2 = graph.AddNode(new SampleNode("A2"));

            var link1 = graph.AddLink(nodeA1, nodeA2, new SampleLink("1"));
            var link2 = graph.AddLink(nodeA1, nodeA2, new SampleLink("2"));

            // act
            var view = graph.PrepareView(x => x.Id.StartsWith("A"), x => ((SampleLink)x).Name != "1");

            // assert
            var nodeA1View = view.Nodes.Single(x => x.Node == nodeA1);

            Assert.That(nodeA1View.OutboundLinks, Has.No.Member(link1));
        }

        [Test]
        public void ShouldNotIncludeLinksWithEndNotMatchingPredicate()
        {
            // arrange
            var graph = new Graph();

            var nodeA1 = graph.AddNode(new SampleNode("A1"));
            var nodeA2 = graph.AddNode(new SampleNode("A2"));

            var nodeB1 = graph.AddNode(new SampleNode("B1"));
            var nodeB2 = graph.AddNode(new SampleNode("B2"));

            var link1 = graph.AddLink(nodeA1, nodeB2, new SampleLink("1"));
            var link2 = graph.AddLink(nodeB1, nodeA2, new SampleLink("2"));
            var link3 = graph.AddLink(nodeA1, nodeA2, new SampleLink("3"));

            // act
            var view = graph.PrepareView(x => x.Id.StartsWith("A"), x => true);

            // assert
            Assert.That(view.Links, Has
                .No.Member(link1)
                .And.No.Member(link2)
                .And.Member(link3));
        }

        public class SampleLink : Link
        {
            public string Name { get; private set; }

            public SampleLink(string name)
            {
                this.Name = name;
            }
        }

        private class SampleNode : Node
        {
            public SampleNode(string nodeId)
                : base(nodeId)
            {
            }
        }
    }
}
