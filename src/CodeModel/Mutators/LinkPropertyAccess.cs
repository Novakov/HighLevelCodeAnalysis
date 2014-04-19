using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;
using Mono.Reflection;

namespace CodeModel.Mutators
{
    public class LinkPropertyAccess : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (node.Method.GetMethodBody() == null)
            {
                return;
            }

            var instructions = from instruction in node.Method.GetInstructions()
                               where instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt
                               let method = instruction.Operand as MethodInfo
                               where method != null && method.IsSpecialName
                               select instruction;

            foreach (var instruction in instructions)
            {
                var method = (MethodInfo)instruction.Operand;

                var property = method.ReflectedType.GetProperties(method.GetBindingFlags() | BindingFlags.Public).FirstOrDefault(x => (x.CanRead && x.GetMethod == method) || (x.CanWrite && x.SetMethod == method));

                if (property != null)
                {
                    var propertyNode = context.FindNodes<PropertyNode>(x => x.Property == property).SingleOrDefault();

                    if (propertyNode != null)
                    {
                        if (property.GetMethod == method)
                        {
                            context.AddLink(node, propertyNode, new GetPropertyLink());
                        }
                        else
                        {
                            context.AddLink(node, propertyNode, new SetPropertyLink());
                        }
                    }
                }
            }
        }
    }
}