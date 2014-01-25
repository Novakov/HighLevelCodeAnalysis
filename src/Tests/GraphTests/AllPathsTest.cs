using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class AllPathsTest
    {
        [Test]
        public void TwoConnectionsWithDifferentHopsAndOneJoinConnectedToEnd()
        {
            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var end = graph.AddNode(new SampleNode("end"));
            var hop1 = graph.AddNode(new SampleNode("hop1"));
            var hop2 = graph.AddNode(new SampleNode("hop2"));
            var join = graph.AddNode(new SampleNode("join"));

            var l1 = graph.AddLink(start, hop1, new SampleLink());
            var l2 = graph.AddLink(start, hop2, new SampleLink());
            var l3 = graph.AddLink(hop1, join, new SampleLink());
            var l4 = graph.AddLink(hop2, join, new SampleLink());
            var l5 = graph.AddLink(join, end, new SampleLink());

            // act
            var paths = FindAllPaths.BetweenNodes(graph, start, end).ToList();

            // assert
            Assert.That(paths, Has.Count.EqualTo(2));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] {l1, l3, l5}));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] {l2, l4, l5}));
        }

        [Test]
        public void TwoConnectionsOneWithTwoHopsSecondWithOneHopLastHopShared()
        {
            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var end = graph.AddNode(new SampleNode("end"));
            var hop1 = graph.AddNode(new SampleNode("hop1"));
            var hop2 = graph.AddNode(new SampleNode("hop2"));            
            
            var l1 = graph.AddLink(start, hop1, new SampleLink());
            var l2 = graph.AddLink(start, hop2, new SampleLink());            
            var l3 = graph.AddLink(hop1, hop2, new SampleLink());
            var l4 = graph.AddLink(hop2, end, new SampleLink());

            // act
            var paths = FindAllPaths.BetweenNodes(graph, start, end).ToList();

            // assert
            Assert.That(paths, Has.Count.EqualTo(2));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { l1, l3, l4 }));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { l2, l4 }));
        }

        [Test]
        [PathTestCase("No connection")]
        [PathTestCase("Direct connection", "start->end")]
        [PathTestCase("One hop", "start->hop->end")]
        [PathTestCase("Two hops", "start->hop1->hop2->end")]
        [PathTestCase("Two connections with one hop", "start->hop1->end", "start->hop2->end")]
        [PathTestCase("One direct connection one with one hop", "start->end", "start->hop->end")]        
        [PathTestCase("Two direct connections", "start->end", "start->end")]        
        public void TestFindingPaths(string[] z)
        {
            var graphPaths = new List<string>(z);

            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var end = graph.AddNode(new SampleNode("end"));

            var nodes = new Dictionary<string, Node>
            {
                {"start", start},
                {"end", end},
            };

            foreach (var graphPath in graphPaths)
            {
                var hops = graphPath.Split(new[] { "->" }, StringSplitOptions.None).ToList();

                for (int i = 1; i < hops.Count; i++)
                {
                    var hop = hops[i];
                    if (!nodes.ContainsKey(hop))
                    {
                        nodes[hop] = graph.AddNode(new SampleNode(hop));
                    }

                    graph.AddLink(nodes[hops[i - 1]], nodes[hop], new SampleLink());
                }
            }

            // act
            var foundPaths = FindAllPaths.BetweenNodes(graph, start, end).ToList();

            foreach (var foundPath in foundPaths)
            {
                Console.WriteLine("Found path: {0}", foundPath.First().Source.Id + "->" + string.Join("->", foundPath.Select(x => x.Target)));
            }

            // assert
            foreach (var foundPath in foundPaths)
            {
                var spec = foundPath.First().Source.Id + "->" + string.Join("->", foundPath.Select(x => x.Target));

                if (!graphPaths.Contains(spec))
                {
                    Assert.Fail("Found path " + spec + " is unknown");
                }
                else
                {
                    graphPaths.Remove(spec);
                }
            }

            foreach (var notFoundPath in graphPaths)
            {
                Assert.Fail("Path " + notFoundPath + " was not found");
            }
        }


        private class SampleLink : Link
        {
        }

        private class SampleNode : Node
        {
            public SampleNode(string nodeId)
                : base(nodeId)
            {
            }
        }

        private class PathTestCaseAttribute : TestCaseAttribute
        {
            public PathTestCaseAttribute(string displayName, params string[] pathSpecs)
                : base(arg: pathSpecs)
            {
                this.TestName = displayName;
            }
        }
    }
}
