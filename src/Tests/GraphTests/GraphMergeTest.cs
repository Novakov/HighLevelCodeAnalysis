using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests.GraphTests
{
    [TestFixture]
    public class GraphMergeTest
    {
        [Test]
        public void MergeDisjointNodes()
        {
            // arrange
            var graph1 = new Graph();
            var node1 = graph1.AddNode(new SimpleNode("1"));
            var node2 = graph1.AddNode(new SimpleNode("2"));

            var graph2 = new Graph();
            var node3 = graph2.AddNode(new SimpleNode("3"));
            var node4 = graph2.AddNode(new SimpleNode("4"));

            // act
            graph1.Merge(graph2);

            // assert
            Assert.That(graph1.Nodes, Has.Member(node1)
                .And.Member(node2)
                .And.Member(node3)
                .And.Member(node4));

            VerifyGraph(graph1);
        }

        [Test]
        public void MergeDisjointNodesWithLinks()
        {
            // arrange
            var graph1 = new Graph();
            var node1 = graph1.AddNode(new SimpleNode("1"));
            var node2 = graph1.AddNode(new SimpleNode("2"));

            var graph2 = new Graph();
            var node3 = graph2.AddNode(new SimpleNode("3"));
            var node4 = graph2.AddNode(new SimpleNode("4"));

            // act
            graph1.Merge(graph2);

            // assert
            Assert.That(graph1.Nodes, Has.Member(node1)
                .And.Member(node2)
                .And.Member(node3)
                .And.Member(node4));

            VerifyGraph(graph1);
        }

        [Test]
        public void MergeWithSharedNodes()
        {
            // arrange
            var graph1 = new Graph();
            var node1 = graph1.AddNode(new SimpleNode("1"));
            var node2 = graph1.AddNode(new SimpleNode("2"));
            var shared1 = graph1.AddNode(new SimpleNode("Shared"));

            var graph2 = new Graph();
            var node3 = graph2.AddNode(new SimpleNode("3"));
            var node4 = graph2.AddNode(new SimpleNode("4"));
            var shared2 = graph1.AddNode(new SimpleNode("Shared"));

            // act
            graph1.Merge(graph2);

            // assert
            VerifyGraph(graph1);

            Assert.That(graph1.Nodes, Has.Member(node1)
                .And.Member(node2)
                .And.Member(node3)
                .And.Member(node4)
                .And.Member(shared1));
        }

        [Test]
        public void MergeWithLinksToSharedNode()
        {
            // arrange
            var graph1 = new Graph();
            var node1 = graph1.AddNode(new SimpleNode("1"));
            var node2 = graph1.AddNode(new SimpleNode("2"));
            var shared1 = graph1.AddNode(new SimpleNode("Shared"));

            var link1 = graph1.AddLink(node1, shared1, new SimpleLink());
            var link2 = graph1.AddLink(shared1, node2, new SimpleLink());

            var graph2 = new Graph();
            var node3 = graph2.AddNode(new SimpleNode("3"));
            var node4 = graph2.AddNode(new SimpleNode("4"));
            var shared2 = graph2.AddNode(new SimpleNode("Shared"));

            var link3 = graph2.AddLink(node3, shared2, new SimpleLink());
            var link4 = graph2.AddLink(shared2, node4, new SimpleLink());

            // act
            graph1.Merge(graph2);

            // assert
            VerifyGraph(graph1);
        }

        [Test]
        public void MergeWithLinksBetweenSharedLinks()
        {
            // arrange
            var graph1 = new Graph();                        
            var shared1 = graph1.AddNode(new SimpleNode("Shared1"));
            var shared2 = graph1.AddNode(new SimpleNode("Shared2"));

            var link1 = graph1.AddLink(shared2, shared1, new SimpleLink());
            var link2 = graph1.AddLink(shared1, shared2, new SimpleLink());

            var graph2 = new Graph();            
            var shared3 = graph2.AddNode(new SimpleNode("Shared3"));
            var shared4 = graph2.AddNode(new SimpleNode("Shared4"));

            var link3 = graph2.AddLink(shared3, shared2, new SimpleLink());
            var link4 = graph2.AddLink(shared2, shared4, new SimpleLink());

            // act            
            graph1.Merge(graph2);

            // assert
            VerifyGraph(graph1);
        }


        public static void VerifyGraph(Graph graph)
        {
            var nodes = new HashSet<Node>(graph.Nodes, new ReferenceEqualityComparer<Node>());
            var remainingLinks = new HashSet<Link>(graph.Links, new ReferenceEqualityComparer<Link>());

            foreach (var node in nodes)
            {
                foreach (var link in node.OutboundLinks)
                {
                    Assert.That(link.Source, Is.SameAs(node), string.Format("Node {0} contains outbound link with source not same as itself", node));
                    Assert.That(nodes.Contains(link.Target), string.Format("Link target {0} does not belong to graph's node set", link.Target));

                    Assert.That(remainingLinks.Contains(link), Is.True, string.Format("Link {0} is not present in graph's link set", link));
                    remainingLinks.Remove(link);
                }

                foreach (var link in node.InboundLinks)
                {
                    Assert.That(link.Target, Is.SameAs(node), string.Format("Node {0} contains inbound link with target not same as itself", node));
                    Assert.That(nodes.Contains(link.Source), string.Format("Link source {0} does not belong to graph's node set", link.Target));

                    Assert.That(graph.Links.Contains(link), Is.True, string.Format("Link {0} is not present in graph's link set", link));                    
                }
            }

            Assert.That(remainingLinks, Is.Empty, "Graph's link set contains link not reachable from nodes");
        }

        private class SimpleNode : Node
        {
            public SimpleNode(string nodeId)
                : base(nodeId)
            {
            }
        }

        private class SimpleLink : Link
        {
        }
    }

    internal class ReferenceEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        public bool Equals(T x, T y)
        {
            return object.ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
