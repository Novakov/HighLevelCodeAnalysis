using CodeModel.Graphs;

namespace CodeModel.Rules
{
    public interface IGraphRule : IRule
    {
        void Verify(VerificationContext context, Graph graph);
    }
}