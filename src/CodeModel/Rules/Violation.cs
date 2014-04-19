using CodeModel.Graphs;

namespace CodeModel.Rules
{
    public class Violation
    {
        public Node Node { get; private set; }

        public object Rule { get; private set; }
        public string Category { get; private set; }

        public Violation(object rule, Node node, string category)
        {
            this.Rule = rule;
            this.Category = category;
            this.Node = node;
        }
    }
}