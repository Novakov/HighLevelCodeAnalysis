using CodeModel;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class WalkAndAnnotateTest
    {
        [Test]
        public void ShouldAnnotateNodesAndLinksReachableFromGivenPoint()
        {
            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var node1 = graph.AddNode(new SampleNode("node1"));
            var node2 = graph.AddNode(new SampleNode("node2"));
            var node3 = graph.AddNode(new SampleNode("node3"));
            var unreachable = graph.AddNode(new SampleNode("unreachable"));

            var link1 = graph.AddLink(start, node1, new SampleLink());
            var link2 = graph.AddLink(node1, node2, new SampleLink());
            var link3 = graph.AddLink(node2, node3, new SampleLink());
            
            // act
            var walkAndAnnotate = new WalkAndAnnotate<Node, Link>(_ => "node annotation", _ => "link annotation");
            walkAndAnnotate.Walk(graph, start);

            // assert
            Assert.That(start.HasAnnotation<string>(), Is.True);
            Assert.That(node1.HasAnnotation<string>(), Is.True);
            Assert.That(node2.HasAnnotation<string>(), Is.True);
            Assert.That(node3.HasAnnotation<string>(), Is.True);
            
            Assert.That(unreachable.HasAnnotation<string>(), Is.False);

            Assert.That(link1.HasAnnotation<string>(), Is.True);
            Assert.That(link2.HasAnnotation<string>(), Is.True);
            Assert.That(link3.HasAnnotation<string>(), Is.True);
        }

        private class SampleNode : Node
        {
            public SampleNode(string nodeId)
                : base(nodeId)
            {
            }
        }

        private class SampleLink : Link
        {
        }
    }
}
