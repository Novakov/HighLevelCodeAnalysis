using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;

namespace CodeModel.Builder
{
    public class MutateContext : IMutateContext
    {
        private readonly Graph model;
        private readonly ISet<Node> nodesToProcess;

        public MutateContext(Graph model)
        {
            this.model = model;

            this.nodesToProcess = new HashSet<Node>(model.Nodes);
        }

        public IEnumerable<Node> NodesToProcess()
        {
            while (this.nodesToProcess.Count > 0)
            {
                var node = this.nodesToProcess.First();
                this.nodesToProcess.Remove(node);

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
    }
}
