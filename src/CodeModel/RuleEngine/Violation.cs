using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.RuleEngine
{  
    public abstract class Violation
    {       
        public IRule Rule { get; internal set; }

        public string Name
        {
            get { return this.GetType().Name; }
        }
    }

    public interface IViolationWithSourceLocation
    {
        SourceLocation? SourceLocation { get; }
    }

    public interface INodeViolation
    {
        Node Node { get; }
    }
}