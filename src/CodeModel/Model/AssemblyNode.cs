using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.Model
{
    public class AssemblyNode : Node
    {
        public Assembly Assembly { get; private set; }

        public AssemblyNode(Assembly assembly) 
            : base(assembly.GetName().ToString())
        {
            this.Assembly = assembly;
        }
    }
}