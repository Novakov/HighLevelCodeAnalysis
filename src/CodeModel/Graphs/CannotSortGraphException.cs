using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeModel.Graphs
{
    [Serializable]
    public class CannotSortGraphException : Exception
    {
        public IEnumerable<Node> UnsortableNodes { get; private set; }
        public IEnumerable<Node> SortedNodes { get; private set; }

        public CannotSortGraphException(IEnumerable<Node> unsortableNodes, IEnumerable<Node> sortedNodes)
            : base("Graph cannot be sorted")
        {
            UnsortableNodes = unsortableNodes;
            SortedNodes = sortedNodes;
        }

        protected CannotSortGraphException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}