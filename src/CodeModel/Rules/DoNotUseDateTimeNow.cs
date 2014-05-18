using System;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Model;
using Mono.Reflection;

namespace CodeModel.Rules
{
    public class DoNotUseDateTimeNow : INodeRule
    {
        public const string UsesDateTimeNow = "UsesDateTimeNow";
        
        public void Verify(VerificationContext context, Node node)
        {
            var methodNode = (MethodNode)node;

            var forbiddenMethods = new[]
            {
                typeof (DateTime).GetProperty("Now").GetMethod,
                typeof (DateTime).GetProperty("UtcNow").GetMethod,
                typeof (DateTimeOffset).GetProperty("Now").GetMethod,
                typeof (DateTimeOffset).GetProperty("UtcNow").GetMethod,
            };

            var violatingInstructions = from instruction in methodNode.Method.GetInstructions()
                where instruction.IsCall()
                let callee = (MethodInfo) instruction.Operand
                where forbiddenMethods.Contains(callee)
                select instruction;

            foreach (var violatingInstruction in violatingInstructions)
            {
                context.RecordViolation(this, node, UsesDateTimeNow);
            }
        }

        public bool IsApplicableTo(Node node)
        {
            return node is MethodNode;
        }
    }
}