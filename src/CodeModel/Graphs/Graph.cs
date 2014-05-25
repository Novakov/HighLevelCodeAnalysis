using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class Graph<TBaseNode, TBaseLink>
        where TBaseNode : Node
        where TBaseLink : Link
    {
        private readonly IDictionary<string, TBaseNode> nodes;
        private readonly ISet<TBaseLink> links;

        public IEnumerable<TBaseNode> Nodes { get { return this.nodes.Values; } }
        public IEnumerable<TBaseLink> Links { get { return this.links; } }

        public Graph()
        {
            this.nodes = new Dictionary<string, TBaseNode>();
            this.links = new HashSet<TBaseLink>();
        }

        public TBaseNode AddNode(TBaseNode node)
        {
            this.nodes.Add(node.Id, node);

            return node;
        }

        public TBaseLink AddLink(TBaseNode source, TBaseNode target, TBaseLink link)
        {
            link.SetUpConnection(source, target);

            source.AddOutboundLink(link);
            target.AddInboundLink(link);

            this.links.Add(link);

            return link;
        }

        public void RemoveLink(TBaseLink link)
        {
            link.Source.RemoveOutboundLink(link);
            link.Target.RemoveInboundLink(link);

            this.links.Remove(link);
        }

        public void ReplaceLink(TBaseLink old, TBaseLink replaceWith)
        {
            replaceWith.SetUpConnection(old.Source, old.Target);

            old.Source.RemoveOutboundLink(old);
            old.Target.RemoveInboundLink(old);

            old.Source.AddOutboundLink(replaceWith);
            old.Target.AddInboundLink(replaceWith);

            this.links.Remove(old);
            this.links.Add(replaceWith);
        }

        public virtual void ReplaceNode(TBaseNode old, TBaseNode replaceWith)
        {
            foreach (var inboundLink in old.InboundLinks)
            {
                inboundLink.SetUpConnection(inboundLink.Source, replaceWith);
                replaceWith.AddInboundLink(inboundLink);
            }

            foreach (var outboundLink in old.OutboundLinks)
            {
                outboundLink.SetUpConnection(replaceWith, outboundLink.Target);
                replaceWith.AddOutboundLink(outboundLink);
            }

            this.nodes.Remove(old.Id);
            this.nodes[replaceWith.Id] = replaceWith;
        }

        public void RemoveNode(TBaseNode node)
        {
            foreach (TBaseLink link in node.InboundLinks.Union(node.OutboundLinks).ToList())
            {
                this.RemoveLink(link);
            }

            this.nodes.Remove(node.Id);
        }

        public void Merge(Graph<TBaseNode, TBaseLink> otherGraph)
        {
            foreach (var node in otherGraph.Nodes)
            {
                if (!this.nodes.ContainsKey(node.Id))
                {
                    this.nodes[node.Id] = node;
                }
            }

            foreach (var link in otherGraph.links)
            {
                var source = this.LookupNode<TBaseNode>(link.Source.Id);
                var target = this.LookupNode<TBaseNode>(link.Target.Id);

                link.SetUpConnection(source, target);
                source.AddOutboundLink(link);
                target.AddInboundLink(link);

                this.links.Add(link);
            }
        }

        public void MoveOutboundLinks(Node from, Node to)
        {
            foreach (var link in from.OutboundLinks.ToList())
            {
                from.RemoveOutboundLink(link);
                to.AddOutboundLink(link);

                link.SetUpConnection(to, link.Target);
            }
        }

        public TNode LookupNode<TNode>(string id)
            where TNode : TBaseNode
        {
            TBaseNode node;
            if (this.nodes.TryGetValue(id, out node))
            {
                return node as TNode;
            }
            else
            {
                return null;
            }
        }

        public GraphView<TBaseNode, TBaseLink> PrepareView(Func<TBaseNode, bool> nodesPredicate, Func<TBaseLink, bool> linksPredicate)
        {
            return new GraphView<TBaseNode, TBaseLink>(this, nodesPredicate, linksPredicate);
        }
    }

    public class Graph : Graph<Node, Link>
    {
    }
}
