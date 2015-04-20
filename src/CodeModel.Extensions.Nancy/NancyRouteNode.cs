using System.Reflection;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Nancy
{
    public class NancyRouteNode : MethodNode
    {
        public string Path { get; private set; }
        public string BuilderName { get; private set; }

        public override string DisplayLabel
        {
            get { return string.Format("{0} {1} -> {2}", this.BuilderName, this.Path, this.Method.Name); }
        }

        public NancyRouteNode(MethodInfo method, string path, string builderName) : base(method)
        {
            Path = path;
            BuilderName = builderName;
        }        
    }
}