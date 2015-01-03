using CodeModel.Graphs;

namespace CodeModel.RuleEngine
{
    public interface IRuleWithBootstrap
    {
        void Initialize(VerificationContext context, Graph graph);
    }
}