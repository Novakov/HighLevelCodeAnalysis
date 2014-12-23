using System;
using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class MethodNode : Node
    {
        private readonly Lazy<string> displayLabel;
        public MethodInfo Method { get; private set; }

        public override string DisplayLabel {
            get { return this.displayLabel.Value; }
        }

        public MethodNode(MethodInfo method) 
            : base(IdFor(method))
        {
            this.Method = method;
            this.displayLabel = new Lazy<string>(() => this.Method.DisplayLabel());
        }

        [Exportable]
        public Type DeclaringType { get { return this.Method.DeclaringType; } }

        public static string IdFor(MethodInfo method)
        {
            return method.DeclaringType.AssemblyQualifiedName + "_" + method.MetadataToken;
        }
    }
}