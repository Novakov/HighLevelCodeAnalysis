using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;

namespace CodeModel.Rules
{
    public interface INodeRule :IRule
    {
        IEnumerable<Violation> Verify(VerificationContext context, Node node);
        bool IsApplicableTo(Node node);
    }
}
