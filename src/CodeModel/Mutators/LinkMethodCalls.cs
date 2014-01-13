using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;
using Mono.Reflection;

namespace CodeModel.Mutators
{
    public class LinkMethodCalls : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            var body = node.Method.GetMethodBody();

            if (body == null)
            {
                return;
            }

            var calledMethods = from instr in node.Method.GetInstructions()
                where
                    instr.OpCode == OpCodes.Call
                    || instr.OpCode == OpCodes.Callvirt
                where instr.Operand is MethodInfo
                select (MethodInfo) instr.Operand;

            foreach (var calledMethod in calledMethods)
            {
                var targetMethod = calledMethod;

                if (calledMethod.IsGenericMethod)
                {
                    targetMethod = calledMethod.GetGenericMethodDefinition();
                }

                var targetNode = context.FindNodes<MethodNode>(x => x.Method == targetMethod).SingleOrDefault();

                if (targetNode != null)
                {
                    context.AddLink(node, targetNode, new MethodCallLink(calledMethod.GetGenericArguments()));
                }
            }
        }
    }
}