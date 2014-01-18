using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class FieldNode : Node
    {
        public FieldInfo Field { get; private set; }

        public FieldNode(FieldInfo field) 
            : base(field.DeclaringType.AssemblyQualifiedName + "_" + field.MetadataToken)
        {
            this.Field = field;
        }
    }
}