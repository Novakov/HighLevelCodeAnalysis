using CodeModel.Graphs;

namespace CodeModel.Extensions.DomainModel
{
    public class HasOneEntityLink : ReferenceLink
    {
        public Node Via { get; private set; }

        public HasOneEntityLink(Node via)
        {
            Via = via;
        }

        public override string ToString()
        {
            return string.Format("{0} --({1})--> {2}", this.Source, this.Via, this.Target);
        }
    }
}