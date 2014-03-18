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
            : base(IdFor(method))
        {
            this.Method = method;
        }

        public static string IdFor(MethodInfo method)
        {
            return method.DeclaringType.AssemblyQualifiedName + "_" + method.MetadataToken;
        }
    }
}