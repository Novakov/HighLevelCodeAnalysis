using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Nancy.Mutators
{
    public class DetectNancyModules : INodeMutator<TypeNode>
    {
        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (node is NancyModuleNode)
            {
                return;
            }

            var nancyAssemblyName = node.Type.Assembly.GetReferencedAssemblies().SingleOrDefault(x => x.Name == "Nancy");

            if (nancyAssemblyName != null)
            {
                var nancyAssembly = Assembly.Load(nancyAssemblyName);
                var nancyModuleType = nancyAssembly.GetType("Nancy.NancyModule");

                if (nancyModuleType.IsAssignableFrom(node.Type))
                {
                    context.ReplaceNode(node, new NancyModuleNode(node.Type));
                }
            }
            // TODO: raise warning
        }
    }
}