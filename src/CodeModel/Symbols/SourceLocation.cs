namespace CodeModel.Symbols
{
    public struct SourceLocation
    {
        private readonly string fileName;
        private readonly int startLine;
        private readonly int startColumn;
        private readonly int endLine;
        private readonly int endColumn;
        public static readonly SourceLocation NotFound = new SourceLocation("<NOT FOUND>", 0, 0, 0, 0);

        public SourceLocation(string fileName, int startLine, int startColumn, int endLine, int endColumn)
        {
            this.fileName = fileName;
            this.startLine = startLine;
            this.startColumn = startColumn;
            this.endLine = endLine;
            this.endColumn = endColumn;
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public int StartLine
        {
            get { return this.startLine; }
        }

        public int StartColumn
        {
            get { return this.startColumn; }
        }

        public int EndLine
        {
            get { return this.endLine; }
        }

        public int EndColumn
        {
            get { return this.endColumn; }
        }
    }
}