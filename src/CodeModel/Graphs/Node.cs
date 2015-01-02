using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public abstract class Node : IEquatable<Node>, IAnnotable
    {
        private readonly ISet<Link> inbound;
        private readonly ISet<Link> outbound;
        private readonly List<object> annotations;

        public string Id { get; private set; }

        public IEnumerable<Link> InboundLinks { get { return this.inbound; } }
        public IEnumerable<Link> OutboundLinks { get { return this.outbound; } }

        public virtual string DisplayLabel
        {
            get { return this.Id; }
        }

        public IEnumerable<object> Annotations { get { return this.annotations; } }

        protected Node(string nodeId)
        {
            this.Id = nodeId;

            this.inbound = new HashSet<Link>();
            this.outbound = new HashSet<Link>();

            this.annotations = new List<object>();
        }

        public bool Equals(Node other)
        {
            return !object.ReferenceEquals(other, null) && other.GetType() == this.GetType() && this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;

            return !object.ReferenceEquals(node, null) && this.Equals(node);
        }

        public static bool operator ==(Node left, Node right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
            {
                return true;
            }

            if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return this.DisplayLabel;
        }

        public bool HasLinkTo(Node target)
        {
            return this.outbound.Any(x => x.Target == target);
        }

        internal void AddOutboundLink(Link link)
        {
            this.outbound.Add(link);
        }

        internal void AddInboundLink(Link link)
        {
            this.inbound.Add(link);
        }

        internal void RemoveOutboundLink(Link link)
        {
            this.outbound.Remove(link);
        }

        internal void RemoveInboundLink(Link link)
        {
            this.inbound.Remove(link);
        }

        public void Annonate(object annotation)
        {
            this.annotations.Add(annotation);
        }

        public void RemoveAnnotation(object annotation)
        {
            this.annotations.Remove(annotation);
        }

        public TAnnotation Annotation<TAnnotation>()
        {
            return this.annotations.OfType<TAnnotation>().SingleOrDefault();
        }
    }
}
