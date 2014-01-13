using System.Collections.Generic;
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

        private class Node1 : Node
        {
            public Node1(string nodeId) : base(nodeId)
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