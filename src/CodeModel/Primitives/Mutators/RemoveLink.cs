using System;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Graphs;

namespace CodeModel.Primitives.Mutators
{
    //TODO: add test
    public class RemoveLink<TLink> : IGraphMutator
        where TLink : Link
    {
        private readonly Func<TLink, bool> predicate;

        public RemoveLink(Func<TLink, bool> predicate)
        {
            this.predicate = predicate;
        }

        public void Mutate(Graph model)
        {
            var linksToRemove = model.Links.OfType<TLink>().Where(this.predicate).ToList();

            foreach (var link in linksToRemove)
            {
                model.RemoveLink(link);
            }
        }
    }
}
