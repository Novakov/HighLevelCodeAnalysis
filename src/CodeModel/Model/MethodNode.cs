using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class MethodNode : Node
    {
        public MethodInfo Method { get; private set; }

        public override string DisplayLabel
        {
            get { return this.Method.ToString(); }
        }

        public MethodNode(MethodInfo method) 
            : base(method.DeclaringType.AssemblyQualifiedName + "_" + method.MetadataToken)
        {
            this.Method = method;
        }
    }
}