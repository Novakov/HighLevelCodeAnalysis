using System.Collections.Generic;
using System.Runtime.InteropServices;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void TwoNodesOfTheSameTypeWithTheSameIdShouldBeEqual()
        {
            // arrange
            var node1 = new Node1("node1");
            var node2 = new Node1("node1");

            // act
            var equal = EqualityComparer<Node>.Default.Equals(node1, node2);

            // Assert
            Assert.That(equal, Is.True, "Nodes are not equal");
        }

        [Test]
        public void TwoNodesOfDifferenceTypeWithTheSameIdShouldNotBeEqual()
        {
            // arrange
            var node1 = new Node1("node1");
            var node2 = new Node2("node1");

            // act
            var equal = EqualityComparer<Node>.Default.Equals(node1, node2);

            // Assert
            Assert.That(equal, Is.False, "Nodes are equal");
        }

        [Test]
        [TestCase("a", "b", false)]
        [TestCase("a", "a", true)]
        [TestCase(null, "a", false)]
        [TestCase("a", null, false)]
        [TestCase(null, null, true)]
        public void EqualityOperatorShouldWork(string id1, string id2, bool expectedEquality)
        {
            // arrange
            var node1 = id1 == null ? null : new Node1(id1);
            var node2 = id2 == null ? null : new Node1(id2);

            // act
            var equality = node1 == node2;
            var inequality = node1 != node2;

            // assert
            Assert.That(equality, Is.EqualTo(expectedEquality), "Equality operator mismatch");
            Assert.That(inequality, Is.EqualTo(!expectedEquality), "Inequality operator mismatch");
        }

        private class Node1 : Node
        {
            public Node1(string nodeId)
                : base(nodeId)
            {
            }
        }

        private class Node2 : Node
        {
            public Node2(string nodeId)
                : base(nodeId)
            {
            }
        }
    }
}