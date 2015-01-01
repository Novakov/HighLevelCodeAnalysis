using System.Collections.Generic;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;
using CodeModel.Rules;
using Mono.Reflection;

namespace CodeModel.Extensions.Cqrs.Rules
{
    [Need(CqrsResources.CountedCommandExecutions)]
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
}