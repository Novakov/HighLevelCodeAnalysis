using System;
using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel.Builder
{
    public interface IMutateContext
    {
        TNode AddNode<TNode>(TNode node)
            where TNode : Node;

        void RemoveNode(Node node);

        void ReplaceNode(Node oldNode, Node newNode);

        IEnumerable<TNode> FindNodes<TNode>(Func<TNode, bool> predicate)
            where TNode : Node;

        TNode FindNode<TNode>(Func<TNode, bool> predicate);
            
        TLink AddLink<TLink>(Node source, Node target, TLink link)
            where TLink : Link;

        void ReplaceLink(Link old, Link replaceWith);

        void RemoveLink(Link link);

        TNode LookupNode<TNode>(string id)
            where TNode : Node;
    }
}