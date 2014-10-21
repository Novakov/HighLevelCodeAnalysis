using System;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Rules;
using Mono.Reflection;

namespace CodeModel.Extensions.Cqrs.Rules
{
    public class InvokeOnlyOneCommand : INodeRule
    {
        public const string Category = "InvokeOnlyOneCommand";

        private readonly ICqrsConvention cqrsConvention;

        public InvokeOnlyOneCommand(ICqrsConvention cqrsConvention)
        {
            this.cqrsConvention = cqrsConvention;
        }

        public void Verify(VerificationContext context, Node node)
        {
            var methodNode = (MethodNode)node;
          
            var cfg = ControlFlowGraphFactory.BuildForMethod(methodNode.Method);

            var violated = false;

            var recorder = new RecordCommandExecution(this.cqrsConvention, () => violated = true);

            recorder.Walk(methodNode.Method, cfg);

            if (violated)
            {
                context.RecordViolation(this, node, Category, null);
            }            
        }

        public bool IsApplicableTo(Node node)
        {
            var methodNode = node as MethodNode;
            return methodNode != null
                && methodNode.Method.HasBody();
        }
    }

    class RecordCommandExecution : BaseCfgWalker<int>
    {
        private readonly ICqrsConvention cqrsConvention;
        private readonly Action recordViolation;

        public RecordCommandExecution(ICqrsConvention cqrsConvention, Action recordViolation)
        {
            this.cqrsConvention = cqrsConvention;
            this.recordViolation = recordViolation; 
        }

        protected override int VisitBlock(int alreadyExecutedCommands, BlockNode block)
        {
            var calls = block.Instructions.Where(x => x.IsCall());

            var commandExecutions = calls.Count(x => this.cqrsConvention.IsCommandExecuteMethod((MethodInfo) x.Operand));

            var commandExecutionInBlock = commandExecutions > 0;
            if (commandExecutionInBlock && (alreadyExecutedCommands + commandExecutions) > 1)
            {
                this.recordViolation();
            }

            return alreadyExecutedCommands + commandExecutions;
        }

        protected override int GetInitialState(MethodInfo method, ControlFlowGraph graph)
        {
            return 0;
        }

        public void Walk(MethodInfo method, ControlFlowGraph cfg)
        {
            base.WalkCore(method, cfg);
        }
    }
}