using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class TopologySortTest
    {
        [Test]
        public void ShouldSortGraphTopologically()
        {
            // arrange
            var graph = new Graph<SampleNode, SampleLink>();
            var node1 = graph.AddNode(new SampleNode("1"));
            var node2 = graph.AddNode(new SampleNode("2"));
            var node3 = graph.AddNode(new SampleNode("3"));
            var node4 = graph.AddNode(new SampleNode("4"));
            var node5 = graph.AddNode(new SampleNode("5"));

            graph.AddLink(node1, node2, new SampleLink());
            graph.AddLink(node1, node5, new SampleLink());
            graph.AddLink(node3, node5, new SampleLink());
            graph.AddLink(node5, node4, new SampleLink());

            // act
            var sorted = TopologySort.SortGraph(graph);

            // assert
            Assert.That(sorted, Is.EqualTo(new[] {node3, node1, node5, node4, node2}));
        }

        [Test]
        public void ShouldThrowWhenGraphHaveCycle()
        {
            // arrange
            var graph = new Graph<SampleNode, SampleLink>();
            var node1 = graph.AddNode(new SampleNode("1"));
            var node2 = graph.AddNode(new SampleNode("2"));
            var node3 = graph.AddNode(new SampleNode("3"));
            var node4 = graph.AddNode(new SampleNode("4"));

            graph.AddLink(node4, node1, new SampleLink());
            graph.AddLink(node4, node2, new SampleLink());
            graph.AddLink(node2, node1, new SampleLink());
            graph.AddLink(node1, node2, new SampleLink());
            graph.AddLink(node1, node3, new SampleLink());
            graph.AddLink(node2, node3, new SampleLink());

            // act
            Assert.That(() => TopologySort.SortGraph(graph), Throws.InstanceOf<CannotSortGraphException>());
        }

        private class SampleNode : Node
        {
            public SampleNode(string nodeId) : base(nodeId)
            {
            }
        }

        private class SampleLink : Link
        {
        }
    }
}
