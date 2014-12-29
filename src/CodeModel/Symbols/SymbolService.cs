using System;
using System.IO;
using CodeModel.Primitives;
using Microsoft.Samples.Debugging.CorMetadata.NativeApi;
using Microsoft.Samples.Debugging.CorSymbolStore;
using Microsoft.Samples.Debugging.SymbolStore;

namespace CodeModel.Symbols
{
    public class SymbolService
    {
        private static readonly Guid ImporterIid = new Guid(0x7dac8207, 0xd3ae, 0x4c75, 0x9b, 0x67, 0x92, 0x80, 0x1a, 0x49, 0x7d, 0x44);

        private readonly SymbolBinder binder;

        public SymbolService()
        {
            this.binder = new SymbolBinder();
        }

        public SequencePoint[] GetSequencePointsForMethod(MethodNode method)
        {
            var dispenser = new MetaDataDispenser();

            var assemblyFile = method.Method.DeclaringType.Assembly.Location;

            object importer;
            ((IMetaDataDispenser)dispenser).OpenScope(assemblyFile, 0, ImporterIid, out importer);

            ISymbolReader reader;

            var searchPath = Path.GetDirectoryName(assemblyFile);

            if (binder.TryGetReaderForFile(importer, assemblyFile, searchPath, out reader) == LoadSymbolsResult.Success)
            {
                var methodReader = reader.GetMethod(new SymbolToken(method.Method.MetadataToken));

                return methodReader.GetSequencePoints();
            }
            else
            {
                return new[] {SequencePoint.Empty};
            }
        }
    }
}
