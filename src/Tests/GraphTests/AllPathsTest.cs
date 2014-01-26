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
            var paths = FindAllPaths.BetweenNodes(start, end).ToList();

            // assert
            Assert.That(paths, Has.Count.EqualTo(2));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop1, join, end }));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop2, join, end }));
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
            var paths = FindAllPaths.BetweenNodes(start, end).ToList();

            // assert
            Assert.That(paths, Has.Count.EqualTo(2));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop1, hop2, end }));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop2, end }));
        }

        [Test]
        public void SplitAndJoinOnHop()
        {
            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var end = graph.AddNode(new SampleNode("end"));
            var hop1 = graph.AddNode(new SampleNode("hop1"));
            var split = graph.AddNode(new SampleNode("split"));

            var hop3 = graph.AddNode(new SampleNode("hop3"));
            var hop4 = graph.AddNode(new SampleNode("hop4"));

            var join = graph.AddNode(new SampleNode("join"));
            var hop2 = graph.AddNode(new SampleNode("hop2"));

            var l1 = graph.AddLink(start, hop1, new SampleLink());
            var l2 = graph.AddLink(hop1, split, new SampleLink());
            var l5 = graph.AddLink(split, join, new SampleLink());
            var l6 = graph.AddLink(join, end, new SampleLink());
            var l3 = graph.AddLink(split, hop3, new SampleLink());
            var l4 = graph.AddLink(hop3, join, new SampleLink());

            // act
            var paths = FindAllPaths.BetweenNodes(start, end);

            // assert
            Assert.That(paths, Has.Count.EqualTo(2));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop1, split, hop3, join, end }));
            Assert.That(paths, Has.Exactly(1).EqualTo(new[] { start, hop1, split, join, end }));
        }

        [Test]
        public void TwoBypasses()
        {
            // arrange
            var graph = new Graph();
            var start = graph.AddNode(new SampleNode("start"));
            var split1 = graph.AddNode(new SampleNode("split1"));
            var hop1 = graph.AddNode(new SampleNode("hop1"));
            var join1 = graph.AddNode(new SampleNode("join1"));
            var split2 = graph.AddNode(new SampleNode("split2"));
            var hop2 = graph.AddNode(new SampleNode("hop2"));
            var join2 = graph.AddNode(new SampleNode("join2"));
            var end = graph.AddNode(new SampleNode("end"));

            graph.AddLink(start, split1, new SampleLink());

            graph.AddLink(split1, hop1, new SampleLink());
            graph.AddLink(hop1, join1, new SampleLink());
            graph.AddLink(split1, join1, new SampleLink());

            graph.AddLink(join1, split2, new SampleLink());

            graph.AddLink(split2, hop2, new SampleLink());
            graph.AddLink(hop2, join2, new SampleLink());
            graph.AddLink(split2, join2, new SampleLink());

            graph.AddLink(join2, end, new SampleLink());

            // act
            var paths = FindAllPaths.BetweenNodes(start, end);

            // assert
            Assert.That(paths, Has.Count.EqualTo(4)
                .And.Exactly(1).EqualTo(new[] { start, split1, hop1, join1, split2, hop2, join2, end })
                .And.Exactly(1).EqualTo(new[] { start, split1, join1, split2, hop2, join2, end })
                .And.Exactly(1).EqualTo(new[] { start, split1, hop1, join1, split2, join2, end })
                .And.Exactly(1).EqualTo(new[] { start, split1, join1, split2, join2, end })
                );
        }

        [Test]
        [PathTestCase("No connection")]
        [PathTestCase("Direct connection", "start->end")]
        [PathTestCase("One hop", "start->hop->end")]
        [PathTestCase("Two hops", "start->hop1->hop2->end")]
        [PathTestCase("Two connections with one hop", "start->hop1->end", "start->hop2->end")]
        [PathTestCase("One direct connection one with one hop", "start->end", "start->hop->end")]
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
            var foundPaths = FindAllPaths.BetweenNodes(start, end).ToList();

            foreach (var foundPath in foundPaths)
            {
                Console.WriteLine("Found path: {0}", string.Join("->", foundPath.Select(x => x.Id)));
            }

            // assert
            foreach (var foundPath in foundPaths)
            {
                var spec = string.Join("->", foundPath.Select(x => x.Id));

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
