using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;

namespace CodeModel.Builder
{
    internal class MutateContext : IMutateContext
    {
        private readonly Graph model;
        private readonly List<Node> nodesToProcess;

        public MutateContext(Graph model)
        {
            this.model = model;

            this.nodesToProcess = new List<Node>(model.Nodes);
        }

        public IEnumerable<Node> NodesToProcess()
        {
            while (this.nodesToProcess.Count > 0)
            {
                var node = this.nodesToProcess[0];
                this.nodesToProcess.RemoveAt(0);

                yield return node;
            }
        }

        public TNode AddNode<TNode>(TNode node)
            where TNode : Node
        {
            this.model.AddNode(node);

            this.nodesToProcess.Add(node);

            return node;
        }

        public void RemoveNode(Node node)
        {
            this.nodesToProcess.Remove(node);

            this.model.RemoveNode(node);
        }

        public void ReplaceNode(Node oldNode, Node newNode)
        {
            this.nodesToProcess.Remove(oldNode);
            this.nodesToProcess.Add(newNode);

            this.model.ReplaceNode(oldNode, newNode);
        }

        public IEnumerable<TNode> FindNodes<TNode>(Func<TNode, bool> predicate) where TNode : Node
        {
            return this.model.Nodes.OfType<TNode>().Where(predicate);
        }

        public TLink AddLink<TLink>(Node source, Node target, TLink link) where TLink : Link
        {
            this.model.AddLink(source, target, link);
            return link;
        }

        public void ReplaceLink(Link old, Link replaceWith)
        {
            this.model.ReplaceLink(old, replaceWith);
        }

        public void RemoveLink(Link link)
        {
            this.model.RemoveLink(link);
        }

        public TNode LookupNode<TNode>(string id)
            where TNode : Node
        {
            return this.model.LookupNode<TNode>(id);
        }
    }
}
