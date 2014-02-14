using System.Reflection;
using CodeModel.Model;

namespace CodeModel.Extensions.AspNetMvc
{
    public class ActionNode : MethodNode
    {
        public ActionNode(MethodInfo action) : base(action)
        {
        }
    }
}