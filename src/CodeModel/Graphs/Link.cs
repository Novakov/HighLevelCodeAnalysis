namespace CodeModel.Graphs
{
    public abstract class Link
    {
        public Node Source { get; private set; }
        public Node Target { get; private set; }

        internal void SetUpConnection(Node source, Node target)
        {
            this.Source = source;
            this.Target = target;
        }

        public override string ToString()
        {
            return this.Source + " -> " + this.Target;
        }
    }
}