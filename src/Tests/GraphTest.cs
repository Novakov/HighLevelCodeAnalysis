using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GraphTest
    {
        [Test]
        public void ShouldTrackAddedNodes()
        {
            // arrange
            var graph = new Graph();

            // act
            graph.AddNode(new SampleNode("a"));

            // Assert
            Assert.That(graph.Nodes, Has.Count.EqualTo(1));
        }

        [Test]
        public void ShouldTrackAddedLinks()
        {
            // arrange
            var source = new SampleNode("source");
            var target = new SampleNode("target");
            
            var graph = new Graph();
            graph.AddNode(source);
            graph.AddNode(target);

            var link = new SampleLink();
            
            // act            
            graph.AddLink(source, target, link);

            // Assert
            Assert.That(graph.Links, Has.Count.EqualTo(1));
            Assert.That(link.Source, Is.SameAs(source));
            Assert.That(link.Target, Is.SameAs(target));

            Assert.That(source.OutboundLinks, Has.Member(link));
            Assert.That(target.InboundLinks, Has.Member(link));
        }

        [Test]
        public void ShouldRemoveLinkFromAll()
        {
            // arrange
            var source = new SampleNode("source");
            var target = new SampleNode("target");

            var graph = new Graph();
            graph.AddNode(source);
            graph.AddNode(target);

            var link = new SampleLink();
            graph.AddLink(source, target, link);

            // act
            graph.RemoveLink(link);

            // Assert
            Assert.That(graph.Links, Has.Count.EqualTo(0));            

            Assert.That(source.OutboundLinks, Has.No.Member(link));
            Assert.That(target.InboundLinks, Has.No.Member(link));
        }

        [Test]
        public void ShouldReplaceLinkInAll()
        {
            // arrange
            var source = new SampleNode("source");
            var target = new SampleNode("target");

            var graph = new Graph();
            graph.AddNode(source);
            graph.AddNode(target);

            var firstLink = new SampleLink();

            graph.AddLink(source, target, firstLink);

            var secondLink = new SecondLink();

            // act            
            graph.ReplaceLink(firstLink, secondLink);

            // Assert
            Assert.That(graph.Links, Has.Count.EqualTo(1));
            Assert.That(graph.Links, Has.No.Member(firstLink));
            Assert.That(graph.Links, Has.Member(secondLink));

            Assert.That(secondLink.Source, Is.SameAs(source));
            Assert.That(secondLink.Target, Is.SameAs(target));

            Assert.That(source.OutboundLinks, Has.No.Member(firstLink));
            Assert.That(target.InboundLinks, Has.No.Member(firstLink));
            
            Assert.That(source.OutboundLinks, Has.Member(secondLink));
            Assert.That(target.InboundLinks, Has.Member(secondLink));
        }

        [Test]
        public void ShouldReplaceNodeInAll()
        {
            // arrange
            var graph = new Graph();
            var firstNode = new SampleNode("firstNode");
            var secondNode = new SampleNode("secondNode");
            
            graph.AddNode(firstNode);
            graph.AddNode(secondNode);

            var inboundLink = new SampleLink();
            graph.AddLink(secondNode, firstNode, inboundLink);

            var outboundLink = new SampleLink();
            graph.AddLink(firstNode, secondNode, outboundLink);

            var newNode = new SampleNode("replacedFirstNode");

            // act
            graph.ReplaceNode(firstNode, newNode);

            // Assert
            Assert.That(graph.Nodes, Has.No.Member(firstNode));
            Assert.That(graph.Nodes, Has.Member(newNode));

            Assert.That(inboundLink.Target, Is.SameAs(newNode));
            Assert.That(newNode.InboundLinks, Has.Member(inboundLink));

            Assert.That(outboundLink.Source, Is.SameAs(newNode));
            Assert.That(newNode.OutboundLinks, Has.Member(outboundLink));
        }

        [Test]
        public void ShouldRemoveNodeInAll()
        {
            // arrange
            var graph = new Graph();
            var node1 = new SampleNode("node1");
            var node2 = new SampleNode("node2");

            graph.AddNode(node1);
            graph.AddNode(node2);

            var inboundLink = new SampleLink();
            graph.AddLink(node2, node1, inboundLink);
            
            var outboundLink = new SampleLink();
            graph.AddLink(node1, node2, outboundLink);

            // act
            graph.RemoveNode(node1);

            // assert
            Assert.That(graph.Nodes, Has.No.Member(node1));
            Assert.That(node2.InboundLinks, Has.No.Member(outboundLink));
            Assert.That(node2.OutboundLinks, Has.No.Member(inboundLink));
        }

        private class SampleLink : Link
        {
        }

        private class SecondLink : Link
        {
        }

        private class SampleNode : Node
        {
            public SampleNode(string nodeId) : base(nodeId)
            {
            }
        }
    }
}
