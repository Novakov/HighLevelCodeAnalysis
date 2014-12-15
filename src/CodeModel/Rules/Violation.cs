using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.Rules
{
    public abstract class Violation
    {
        public Node Node { get; private set; }

        public IRule Rule { get; internal set; }

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