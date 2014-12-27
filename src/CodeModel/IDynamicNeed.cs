using System.Collections.Generic;

namespace CodeModel
{
    public interface IDynamicNeed
    {
        IEnumerable<string> NeededResources { get; }
    }
}