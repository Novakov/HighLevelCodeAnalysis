using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public class Graph
    {
        private readonly ISet<Node> nodes; 
        private readonly ISet<Link> links; 

        public IEnumerable<Node> Nodes { get { return this.nodes; } }
        public IEnumerable<Link> Links { get { return this.links; } }

        public Graph()
        {
            this.nodes = new HashSet<Node>();
            this.links = new HashSet<Link>();
        }

        public Node AddNode(Node node)
        {
            this.nodes.Add(node);
            
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

        public void ReplaceNode(Node old, Node replaceWith)
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

            this.nodes.Remove(old);
            this.nodes.Add(replaceWith);
        }

        public void RemoveNode(Node node)
        {
            foreach (var link in node.InboundLinks.Union(node.OutboundLinks).ToList())
            {
                this.RemoveLink(link);
            }

            this.nodes.Remove(node);
        }

        public void Merge(Graph otherGraph)
        {
            this.nodes.UnionWith(otherGraph.nodes);
            foreach (var link in otherGraph.links)
            {
                var source = this.nodes.First(x => x.Id == link.Source.Id);
                var target = this.nodes.First(x => x.Id == link.Target.Id);

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
    }
}
