using System;
using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;

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

        TLink AddLink<TLink>(Node source, Node target, TLink link)
            where TLink : Link;

        void ReplaceLink(Link old, Link replaceWith);

        void RemoveLink(Link link);

        TNode LookupNode<TNode>(string id)
            where TNode : Node;
    }
}