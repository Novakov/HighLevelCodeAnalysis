using System.Linq;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives;

namespace CodeModel.Extensions.AspNetMvc.Mutators
{
    [Provide(AspNetMvcResources.Controllers)]
    [Need(Resources.Types)]
    public class DetectControllers : INodeMutator<TypeNode>
    {
        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (node is ControllerNode)
            {
                return;
            }

            var mvcAssembly = node.Type.Assembly.GetReferencedAssemblies().SingleOrDefault(x => x.Name == "System.Web.Mvc");

            if (mvcAssembly != null)
            {
                var controllerType = mvcAssembly.Load().GetType("System.Web.Mvc.Controller");

                if (controllerType.IsAssignableFrom(node.Type) && !node.Type.IsAbstract && node.Type.IsPublic)
                {
                    context.ReplaceNode(node, new ControllerNode(node.Type));
                }
            }
        }
    }
}