using System.Linq;
using Microsoft.Samples.Debugging.SymbolStore;
using Mono.Reflection;

namespace CodeModel.Symbols
{
    static internal class Extensions
    {
        public static SequencePoint[] GetSequencePoints(this ISymbolMethod method)
        {
            var offsets = new int[method.SequencePointCount];
            var documents = new ISymbolDocument[method.SequencePointCount];
            var lines = new int[method.SequencePointCount];
            var columns = new int[method.SequencePointCount];
            var endLines = new int[method.SequencePointCount];
            var endColumns = new int[method.SequencePointCount];

            var sequencePoints = new SequencePoint[method.SequencePointCount];

            method.GetSequencePoints(offsets, documents, lines, columns, endLines, endColumns);

            for (int i = 0; i < method.SequencePointCount; i++)
            {
                var location = new SourceLocation(documents[i].URL, lines[i], columns[i], endLines[i], endColumns[i]);

                sequencePoints[i] = new SequencePoint(offsets[i], location);
            }
            return sequencePoints;
        }

        public static SequencePoint NearestSequencePoint(this SequencePoint[] sequencePoints, Instruction instruction)
        {
            return sequencePoints.Where(x => x.Offset <= instruction.Offset).OrderByDescending(x => x.Offset).First();
        }
    }
}