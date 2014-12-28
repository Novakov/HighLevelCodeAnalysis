using System.Linq;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Extensions.Cqrs.Rules;
using CodeModel.FlowAnalysis;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
    [Provide(CqrsResources.CountedCommandExecutions)]
    [Need(Resources.Methods)]
    public class CountCommandExecutions : INodeMutator<MethodNode>
    {
        private readonly ICqrsConvention convention;

        public CountCommandExecutions(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {           
            if (node.HasAnnotation<CommandExecutionCount>())
            {
                return;
            }

            if (!node.Method.HasBody())
            {
                return;
            }

            var walker = new RecordCommandExecution(this.convention);

            var cfg = ControlFlowGraphFactory.BuildForMethod(node.Method);

            var commandExecutionCountsOnPaths = walker.Walk(node.Method, cfg);

            var highestCommandExecutionCount = commandExecutionCountsOnPaths.Max();

            node.Annonate(new CommandExecutionCount(highestCommandExecutionCount));
        }
    }
}