using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.Rules
{
    public class Violation
    {
        public Node Node { get; private set; }

        public object Rule { get; private set; }
        public string Category { get; private set; }
        public SourceLocation? SourceLocation { get; private set; }

        public Dictionary<string, object> Attachments { get; private set; }

        public Violation(object rule, Node node, string category, SourceLocation? sourceLocation)
        {
            this.Rule = rule;
            this.Category = category;
            this.SourceLocation = sourceLocation;
            this.Node = node;
            this.Attachments = new Dictionary<string, object>();
        }

        public Violation Attach(string key, object value)
        {
            this.Attachments[key] = value;

            return this;
        }
    }
}