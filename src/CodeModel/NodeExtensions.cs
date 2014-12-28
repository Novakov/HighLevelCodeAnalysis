using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel
{
    public static class NodeExtensions
    {
        public static Node GetContainer(this Node @this)
        {
            return @this.OutboundLinks.OfType<ContainedInLink>().Select(x => x.Target).FirstOrDefault();
        }

        public static IEnumerable<TFrom> InboundFrom<TFrom, TVia>(this Node @this)
            where TFrom : Node
            where TVia : Link
        {
            return @this.InboundLinks.OfType<TVia>().Where(x => x.Source is TFrom).Select(x => (TFrom) x.Source);
        }
    }
}
