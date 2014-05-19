using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using CodeModel.Model;
using Microsoft.Samples.Debugging.CorMetadata.NativeApi;
using Microsoft.Samples.Debugging.CorSymbolStore;
using Microsoft.Samples.Debugging.SymbolStore;
using Mono.Reflection;
using ISymbolReader = Microsoft.Samples.Debugging.SymbolStore.ISymbolReader;
using SymbolToken = Microsoft.Samples.Debugging.SymbolStore.SymbolToken;

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
                var sourceLocation = FindSourceLocation(violatingInstruction, methodNode);

                context.RecordViolation(this, node, UsesDateTimeNow, sourceLocation);
            }
        }

        private SourceLocation? FindSourceLocation(Instruction instruction, MethodNode methodNode)
        {
            var binder = new SymbolBinder();

            var dispenserClassID = new Guid(0xe5cb7a31, 0x7512, 0x11d2, 0x89, 0xce, 0x00, 0x80, 0xc7, 0x92, 0xe5, 0xd8); // CLSID_CorMetaDataDispenser            

            var importerIID = new Guid(0x7dac8207, 0xd3ae, 0x4c75, 0x9b, 0x67, 0x92, 0x80, 0x1a, 0x49, 0x7d, 0x44);

            var dispenserType = Type.GetTypeFromCLSID(dispenserClassID);

            //var dispenser = (IMetaDataDispenser)Activator.CreateInstance(dispenserType);
            var dispenser = new MetaDataDispenser();

            var dllFile = methodNode.Method.DeclaringType.Assembly.Location;

            object importer;
            ((IMetaDataDispenser)dispenser).OpenScope(dllFile, 0, importerIID, out importer);

            ISymbolReader reader;

            var searchPath = Path.GetDirectoryName(dllFile);

            if (binder.TryGetReaderForFile(importer, dllFile, searchPath, out reader) == LoadSymbolsResult.Success)
            {
                var method = reader.GetMethod(new SymbolToken(methodNode.Method.MetadataToken));

                var offsets = new int[method.SequencePointCount];
                var documents = new ISymbolDocument[method.SequencePointCount];
                var lines = new int[method.SequencePointCount];
                var columns = new int[method.SequencePointCount];
                var endLines = new int[method.SequencePointCount];
                var endColumns = new int[method.SequencePointCount];

                method.GetSequencePoints(offsets, documents, lines, columns, endLines, endColumns);

                var spIndex = offsets.Select((x, i) => new {Offset = x, Index = i}).Where(x => x.Offset <= instruction.Offset).OrderByDescending(x => x.Offset).First().Index;

                return new SourceLocation(documents[spIndex].URL, lines[spIndex], columns[spIndex], endLines[spIndex], endColumns[spIndex]);
            }

            return null;
        }

        public bool IsApplicableTo(Node node)
        {
            return node is MethodNode;
        }
    }
}