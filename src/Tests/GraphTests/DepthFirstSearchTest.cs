using System.Collections.Generic;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class DepthFirstSearchTest
    {
        [Test]
        public void ShouldWalkThroughAllNodes()
        {
            // arrange
            var graph = new Graph();
            var a = graph.AddNode(new SampleNode("A"));
            var b = graph.AddNode(new SampleNode("B"));
            var c = graph.AddNode(new SampleNode("C"));
            var d = graph.AddNode(new SampleNode("D"));
            var e = graph.AddNode(new SampleNode("E"));
            var f = graph.AddNode(new SampleNode("F"));
            var g = graph.AddNode(new SampleNode("G"));

            graph.AddLink(a, b, new SampleLink());
            graph.AddLink(a, c, new SampleLink());
            graph.AddLink(a, d, new SampleLink());
            graph.AddLink(c, e, new SampleLink());
            graph.AddLink(e, f, new SampleLink());
            graph.AddLink(e, g, new SampleLink());

            var enterSequence = new List<SampleNode>();
            var leaveSequence = new List<SampleNode>();

            var dfs = new LambdaDepthFirstSearch
            {
                EnteringNode = (node, links) => enterSequence.Add((SampleNode)node),
                LeavingNode = (node, links) => leaveSequence.Add((SampleNode)node)
            };

            // act
            dfs.Walk(a);

            // assert
            Assert.That(enterSequence, Is.EqualTo(new[] {a, b, c, e, f, g, d}), "Enter node sequence is correct");
            Assert.That(leaveSequence, Is.EqualTo(new[] {b, f, g, e, c, d, a}), "Leave node sequence is correct");
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