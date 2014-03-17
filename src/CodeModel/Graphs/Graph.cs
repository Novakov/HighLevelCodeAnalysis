using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class Graph
    {
        private readonly IDictionary<string, Node> nodes; 
        private readonly ISet<Link> links; 

        public IEnumerable<Node> Nodes { get { return this.nodes.Values; } }
        public IEnumerable<Link> Links { get { return this.links; } }

        public Graph()
        {
            this.nodes = new Dictionary<string, Node>();
            this.links = new HashSet<Link>();
        }

        public Node AddNode(Node node)
        {
            this.nodes.Add(node.Id, node);
            
            return node;
        }

        public Link AddLink(Node source, Node target, Link link)
        {
            link.SetUpConnection(source, target);

            source.AddOutboundLink(link);
            target.AddInboundLink(link);

            this.links.Add(link);

            return link;
        }

        public void RemoveLink(Link link)
        {
            link.Source.RemoveOutboundLink(link);
            link.Target.RemoveInboundLink(link);

            this.links.Remove(link);
        }

        public void ReplaceLink(Link old, Link replaceWith)
        {
            replaceWith.SetUpConnection(old.Source, old.Target);

            old.Source.RemoveOutboundLink(old);
            old.Target.RemoveInboundLink(old);

            old.Source.AddOutboundLink(replaceWith);
            old.Target.AddInboundLink(replaceWith);

            this.links.Remove(old);
            this.links.Add(replaceWith);
        }

        public virtual void ReplaceNode(Node old, Node replaceWith)
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

        public void RemoveNode(Node node)
        {
            foreach (var link in node.InboundLinks.Union(node.OutboundLinks).ToList())
            {
                this.RemoveLink(link);
            }

            this.nodes.Remove(node.Id);
        }

        public void Merge(Graph otherGraph)
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
                var source = this.LookupNode<Node>(link.Source.Id);
                var target = this.LookupNode<Node>(link.Target.Id);

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
            where TNode : Node
        {
            Node node;
            if (this.nodes.TryGetValue(id, out node))
            {
                return node as TNode;
            }
            else
            {
                return null;
            }
        }

        public GraphView PrepareView(Func<Node, bool> nodesPredicate, Func<Link, bool> linksPredicate)
        {
            return new GraphView(this, nodesPredicate, linksPredicate);
        }
    }
}
