using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Primitives
{
    public class FieldNode : Node
    {
        public FieldInfo Field { get; private set; }

        public override string DisplayLabel
        {
            get { return this.Field.ToString(); }
        }

        public FieldNode(FieldInfo field) 
            : base(field.DeclaringType.AssemblyQualifiedName + "_" + field.MetadataToken)
        {
            this.Field = field;
        }
    }
}