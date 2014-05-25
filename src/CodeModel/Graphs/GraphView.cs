using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class GraphView<TBaseNode, TBaseLink>
        where TBaseNode : Node
        where TBaseLink : Link
    {
        private readonly Graph<TBaseNode, TBaseLink> graph;
        private readonly Func<TBaseLink, bool> linksPredicate;

        public IEnumerable<NodeView<TBaseNode>> Nodes { get; set; }

        public IEnumerable<TBaseLink> Links
        {
            get { return graph.Links.Where(this.linksPredicate); }
        }

        public GraphView(Graph<TBaseNode, TBaseLink> graph, Func<TBaseNode, bool> nodesPredicate, Func<TBaseLink, bool> linksPredicate)
        {
            this.graph = graph;

            this.linksPredicate = l => linksPredicate(l) && nodesPredicate((TBaseNode)l.Source) && nodesPredicate((TBaseNode)l.Target);

            this.Nodes = graph.Nodes.Where(nodesPredicate)
                .Select(x => new NodeView<TBaseNode>(x, x.InboundLinks.OfType<TBaseLink>().Where(linksPredicate), x.OutboundLinks.OfType<TBaseLink>().Where(linksPredicate)))
                .ToList();
        }
    }

    public class NodeView<TBaseNode>
        where TBaseNode : Node
    {
        public TBaseNode Node { get; private set; }
        public IEnumerable<Link> InboundLinks { get; private set; }
        public IEnumerable<Link> OutboundLinks { get; private set; }

        public NodeView(TBaseNode node, IEnumerable<Link> inboundLinks, IEnumerable<Link> outboundLinks)
        {
            this.Node = node;
            this.InboundLinks = inboundLinks.ToList();
            this.OutboundLinks = outboundLinks.ToList();
        }
    }
}