using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Primitives
{
    public class PropertyNode : Node
    {
        public PropertyInfo Property { get; private set; }

        public override string DisplayLabel
        {
            get { return this.Property.ToString(); }
        }

        public PropertyNode(PropertyInfo property) 
            : base(property.DeclaringType.AssemblyQualifiedName + "_" + property.MetadataToken)
        {
            this.Property = property;
        }
    }
}