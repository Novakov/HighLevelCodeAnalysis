using System.Collections.Generic;

namespace CodeModel.Dependencies
{
    public interface IDynamicNeed
    {
        IEnumerable<string> NeededResources { get; }
    }
}