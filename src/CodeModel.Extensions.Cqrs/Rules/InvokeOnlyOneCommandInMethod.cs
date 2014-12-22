using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Rules;
using Mono.Reflection;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class InvokeOnlyOneCommandInMethod : INodeRule
    {
        private readonly ICqrsConvention cqrsConvention;

        public InvokeOnlyOneCommandInMethod(ICqrsConvention cqrsConvention)
        {
            this.cqrsConvention = cqrsConvention;
        }

        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            var count = node.Annotation<CommandExecutionCount>();

            if (count == null || count.HighestCount <= 1)
            {
                yield break;
            }

            yield return new MethodExecutesMoreThanOneCommandViolation(node);            
        }

        public bool IsApplicableTo(Node node)
        {            
            return node is MethodNode;
        }
    }

    class RecordCommandExecution : BaseCfgWalker<int>
    {
        private readonly ICqrsConvention cqrsConvention;

        public RecordCommandExecution(ICqrsConvention cqrsConvention)
        {
            this.cqrsConvention = cqrsConvention;
        }

        protected override int VisitBlock(int alreadyExecutedCommands, BlockNode block)
        {
            var calls = block.Instructions.Where(x => x.IsCall());

            var commandExecutions = calls.Count(x => x.Operand is MethodInfo && this.cqrsConvention.IsCommandExecuteMethod((MethodInfo) x.Operand));            
            
            return alreadyExecutedCommands + commandExecutions;
        }

        protected override int GetInitialState(MethodInfo method, ControlFlowGraph graph)
        {
            return 0;
        }

        public IEnumerable<int> Walk(MethodInfo method, ControlFlowGraph cfg)
        {
            return base.WalkCore(method, cfg);
        }
    }
}