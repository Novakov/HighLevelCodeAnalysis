using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class PropertyNode : Node
    {
        public PropertyInfo Property { get; private set; }

        public PropertyNode(PropertyInfo property) 
            : base(property.DeclaringType.AssemblyQualifiedName + "_" + property.MetadataToken)
        {
            this.Property = property;
        }
    }
}