using System.Linq;
using CodeModel.Graphs;
using CodeModel.Links;

namespace CodeModel
{
    public static class NodeExtensions
    {
        public static Node GetContainer(this Node @this)
        {
            return @this.OutboundLinks.OfType<ContainedInLink>().Select(x => x.Target).FirstOrDefault();
        }
    }
}
