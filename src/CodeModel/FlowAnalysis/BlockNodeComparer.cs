using System.Collections.Generic;

namespace CodeModel.FlowAnalysis
{
    public class BlockNodeComparer : IComparer<BlockNode>
    {
        private const int Equal = 0;
        private const int XIsGreater = 1;
        private const int YIsGreater = -1;

        public int Compare(BlockNode x, BlockNode y)
        {
            var xAsInstruction = x as InstructionBlockNode;
            var yAsInstruction = y as InstructionBlockNode;

            var xAsExit = x as MethodExitNode;
            var yAsExit = y as MethodExitNode;

            if (xAsExit != null || yAsExit != null)
            {
                if (xAsInstruction != null)
                {
                    return YIsGreater;
                }

                if (yAsInstruction != null)
                {
                    return XIsGreater;
                }

                return Equal;
            }

            return xAsInstruction.First.Offset.CompareTo(yAsInstruction.First.Offset);
        }
    }
}