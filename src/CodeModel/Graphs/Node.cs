﻿using System;
using System.Collections.Generic;

namespace CodeModel.Graphs
{
    public abstract class Node : IEquatable<Node>
    {
        private readonly ISet<Link> inbound;
        private readonly ISet<Link> outbound;

        public string Id { get; private set; }

        public IEnumerable<Link> InboundLinks { get { return this.inbound; } } 
        public IEnumerable<Link> OutboundLinks { get { return this.outbound; } } 

        protected Node(string nodeId)
        {
            this.Id = nodeId;

            this.inbound = new HashSet<Link>();
            this.outbound = new HashSet<Link>();
        }

        public bool Equals(Node other)
        {
            return other != null && other.GetType() == this.GetType() && this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;

            return node != null && this.Equals(node);
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
    }
}
