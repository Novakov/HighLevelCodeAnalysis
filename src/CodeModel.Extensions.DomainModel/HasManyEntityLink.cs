using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel
{
    public class HasManyEntityLink : Link
    {
        public Node Via { get; private set; }

        public HasManyEntityLink(Node via)
        {
            Via = via;
        }

        public override string ToString()
        {
            return string.Format("{0} --({1})--> {2}", this.Source, this.Via, this.Target);
        }
    }
}