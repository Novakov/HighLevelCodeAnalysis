namespace CodeModel.Symbols
{
    public struct SequencePoint
    {
        private readonly int offset;
        private readonly SourceLocation location;
        public static readonly SequencePoint Empty = new SequencePoint(0, SourceLocation.NotFound);

        public SequencePoint(int offset, SourceLocation location)
        {
            this.offset = offset;
            this.location = location;
        }

        public int Offset
        {
            get { return this.offset; }
        }

        public SourceLocation Location
        {
            get { return this.location; }
        }
    }
}