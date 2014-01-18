using System;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class TypeNode : Node
    {
        public Type Type { get; private set; }

        public override string DisplayLabel
        {
            get { return this.Type.FullName; }
        }

        public TypeNode(Type type)
            : base(type.AssemblyQualifiedName + "_" + type.MetadataToken)
        {
            this.Type = type;
        }
    }
}