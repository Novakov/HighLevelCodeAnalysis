using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.RuleEngine
{
    //TODO: Violation can be for graph as a whole, so make Node optional (interface IViolationOnNode)
    public abstract class Violation
    {
        public Node Node { get; private set; }

        public IRule Rule { get; internal set; }

        public string Name
        {
            get { return this.GetType().Name; }
        }

        protected Violation(Node node)
        {
            this.Node = node;            
        }
    }

    public interface IViolationWithSourceLocation
    {
        SourceLocation? SourceLocation { get; }
    }
}