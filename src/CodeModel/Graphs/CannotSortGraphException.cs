using System;
using System.Runtime.Serialization;

namespace CodeModel.Graphs
{
    [Serializable]
    public class CannotSortGraphException : Exception
    {        
        public CannotSortGraphException()
            : base("Graph cannot be sorted")
        {
        }
        
        protected CannotSortGraphException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}