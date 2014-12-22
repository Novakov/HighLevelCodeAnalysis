using System.Collections.Generic;
using CodeModel.Graphs;

namespace CodeModel.Rules
{
    public interface IGraphRule : IRule
    {
        IEnumerable<Violation> Verify(VerificationContext context, Graph graph);
    }
}