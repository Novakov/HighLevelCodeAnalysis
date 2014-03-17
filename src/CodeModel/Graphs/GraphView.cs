using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class GraphView
    {
        private readonly Graph graph;
        private readonly Func<Link, bool> linksPredicate;

        public IEnumerable<NodeView> Nodes { get; set; }

        public IEnumerable<Link> Links
        {
            get { return graph.Links.Where(this.linksPredicate); }
        }

        public GraphView(Graph graph, Func<Node, bool> nodesPredicate, Func<Link, bool> linksPredicate)
        {
            this.graph = graph;

            this.linksPredicate = l => linksPredicate(l) && nodesPredicate(l.Source) && nodesPredicate(l.Target);

            this.Nodes = graph.Nodes.Where(nodesPredicate)
                .Select(x => new NodeView(x, x.InboundLinks.Where(linksPredicate), x.OutboundLinks.Where(linksPredicate)))
                .ToList();
        }
    }

    public class NodeView        
    {
        public Node Node { get; private set; }
        public IEnumerable<Link> InboundLinks { get; private set; }
        public IEnumerable<Link> OutboundLinks { get; private set; }

        public NodeView(Node node, IEnumerable<Link> inboundLinks, IEnumerable<Link> outboundLinks)
        {
            this.Node = node;
            this.InboundLinks = inboundLinks.ToList();
            this.OutboundLinks = outboundLinks.ToList();
        }
    }
}