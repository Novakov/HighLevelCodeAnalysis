using System;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Graphs;

namespace CodeModel.Primitives.Mutators
{
    public class LinkApplicationEntryPointTo<TNode> : IGraphMutator
        where TNode : Node
    {
        private readonly Func<TNode, bool> entryPredicate;

        public LinkApplicationEntryPointTo(Func<TNode, bool> entryPredicate)
        {
            this.entryPredicate = entryPredicate;
        }

        public void Mutate(Graph model)
        {
            var entryPoint = model.Nodes.OfType<ApplicationEntryPoint>().Single();

            var matchingEntryCalls = model.Nodes.OfType<TNode>().Where(this.entryPredicate);

            foreach (var node in matchingEntryCalls)
            {
                model.AddLink(entryPoint, node, new ApplicationEntryCall());
            }
        }
    }
}