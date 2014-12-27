using System.Collections.Generic;
using CodeModel.Graphs;

namespace CodeModel.Convetions
{
    public interface IApplicationEntryPointConvention : IConvention
    {
        bool IsEntryPoint(Node node);
        IEnumerable<string> ResourcesNeededForLinkingEntryPoint { get; }
    }
}