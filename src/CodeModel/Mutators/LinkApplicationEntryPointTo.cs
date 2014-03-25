using System;
using System.Linq;
using System.Text.RegularExpressions;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Mutators
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