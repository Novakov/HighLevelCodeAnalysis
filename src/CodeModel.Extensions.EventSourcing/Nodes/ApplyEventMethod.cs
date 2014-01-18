using System;
using CodeModel.Model;

namespace CodeModel.Extensions.EventSourcing.Nodes
{
    public class ApplyEventMethod : MethodNode
    {
        public Type AppliedEventType { get; private set; }

        public ApplyEventMethod(MethodNode node, Type appliedEventType)
            : base(node.Method)
        {
            this.AppliedEventType = appliedEventType;
        }
    }
}