using System.Collections.Generic;
using CodeModel.Graphs;

namespace CodeModel.RuleEngine
{
    public interface INodeRule : IRule
    {
        IEnumerable<Violation> Verify(VerificationContext context, Node node);
        bool IsApplicableTo(Node node);
    }
}
