using System.Collections.Generic;
using CodeModel.Convetions;
using CodeModel.Graphs;

namespace CodeModel.Mutators
{
    [Provide(Resources.LinksToEntryPoints)]
    [Need(Resources.EntryPoint)]
    [DynamicNeed]
    public class LinkApplicationEntryPoint : LinkApplicationEntryPointTo<Node>, IDynamicNeed
    {
        private readonly IApplicationEntryPointConvention convention;

        public IEnumerable<string> NeededResources
        {
            get { return convention.ResourcesNeededForLinkingEntryPoint; }
        }

        public LinkApplicationEntryPoint(IApplicationEntryPointConvention convention)
            : base(convention.IsEntryPoint)
        {
            this.convention = convention;
        }
    }
}