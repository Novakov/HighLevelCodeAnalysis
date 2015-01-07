using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel
{
    public class AggregateReferenceLink : Link
    {
        public Node Via { get; private set; }

        public AggregateReferenceLink(Node via)
        {
            Via = via;
        }
    }
}