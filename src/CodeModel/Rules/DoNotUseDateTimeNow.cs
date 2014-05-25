using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Symbols;
using Microsoft.Samples.Debugging.CorMetadata.NativeApi;
using Microsoft.Samples.Debugging.CorSymbolStore;
using Microsoft.Samples.Debugging.SymbolStore;
using Mono.Reflection;

namespace CodeModel.Rules
{
    public class DoNotUseDateTimeNow : INodeRule
    {
        public const string UsesDateTimeNow = "UsesDateTimeNow";

        private readonly SymbolService symbols;

        public DoNotUseDateTimeNow(SymbolService symbols)
        {
            this.symbols = symbols;
        }

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
                var sourceLocation = FindSourceLocation(violatingInstruction, methodNode);

                context.RecordViolation(this, node, UsesDateTimeNow, sourceLocation);
            }
        }

        private SourceLocation? FindSourceLocation(Instruction instruction, MethodNode methodNode)
        {            
            var sequencePoints = this.symbols.GetSequencePointsForMethod(methodNode);

            return sequencePoints.NearestSequencePoint(instruction).Location; 
        }

        public bool IsApplicableTo(Node node)
        {
            return node is MethodNode;
        }
    }
}