using System;
using System.Linq;
using CodeModel.Builder;
using CodeModel.Model;

namespace CodeModel.Extensions.AspNetMvc
{
    [Provide(AspNetMvcResources.AspNetMvcActions)]
    [Need(Resources.Methods)]
    public class DetectActions : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (node is ActionNode)
            {
                return;
            }

            var mvcAssembly = node.Method.DeclaringType.Assembly.GetReferencedAssemblies().SingleOrDefault(x => x.Name == "System.Web.Mvc");

            if (mvcAssembly != null)
            {
                var actionResultType = mvcAssembly.Load().GetType("System.Web.Mvc.ActionResult");

                if (actionResultType.IsAssignableFrom(node.Method.ReturnType) && node.GetContainer() is ControllerNode)
                {
                    context.ReplaceNode(node, new ActionNode(node.Method));
                }
            }
        }
    }
}