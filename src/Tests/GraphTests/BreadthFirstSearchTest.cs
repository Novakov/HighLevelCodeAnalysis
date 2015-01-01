using System.Collections.Generic;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class BreadthFirstSearchTest
    {
        [Test]
        public void AcyclicGraph()
        {
            // arrange
            var graph = new Graph();
            
            var a = graph.AddNode(new SampleNode("a"));
            var b = graph.AddNode(new SampleNode("b"));
            var c = graph.AddNode(new SampleNode("c"));
            var d = graph.AddNode(new SampleNode("d"));
            var e = graph.AddNode(new SampleNode("e"));
            var f = graph.AddNode(new SampleNode("f"));
            var g = graph.AddNode(new SampleNode("g"));
            var h = graph.AddNode(new SampleNode("h"));

            graph.AddLink(a, b, new SampleLink());
            graph.AddLink(a, c, new SampleLink());
            graph.AddLink(b, d, new SampleLink());
            graph.AddLink(b, e, new SampleLink());
            graph.AddLink(c, f, new SampleLink());
            graph.AddLink(c, g, new SampleLink());
            graph.AddLink(e, h, new SampleLink());

            // act
            var path = new List<Node>();

            var collectVisitedNodes = new LambdaBreadthFirstSearch<Node, Link>
            {
                HandlingNode = (node, links) => path.Add(node)
            };

            collectVisitedNodes.Walk(graph, a);

            // assert
            Assert.That(path, Is.EqualTo(new[] { a, b, c, d, e, f, g, h }));
        }

        [Test]
        public void CyclicGraph()
        {
            // arrange
            var graph = new Graph();

            var a = graph.AddNode(new SampleNode("a"));
            var b = graph.AddNode(new SampleNode("b"));
            var c = graph.AddNode(new SampleNode("c"));
            var d = graph.AddNode(new SampleNode("d"));
            var e = graph.AddNode(new SampleNode("e"));
            var f = graph.AddNode(new SampleNode("f"));
            var g = graph.AddNode(new SampleNode("g"));
            var h = graph.AddNode(new SampleNode("h"));

            graph.AddLink(a, b, new SampleLink());
            graph.AddLink(a, c, new SampleLink());
            graph.AddLink(b, d, new SampleLink());
            graph.AddLink(b, e, new SampleLink());
            graph.AddLink(c, f, new SampleLink());
            graph.AddLink(c, g, new SampleLink());
            graph.AddLink(e, h, new SampleLink());
            graph.AddLink(e, a, new SampleLink());

            // act
            var path = new List<Node>();

            var collectVisitedNodes = new LambdaBreadthFirstSearch<Node, Link>
            {
                HandlingNode = (node, links) => path.Add(node)
            };

            collectVisitedNodes.Walk(graph, a);

            // assert            
            Assert.That(path, Is.EqualTo(new[] {a, b, c, d, e, f, g, h}));
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
