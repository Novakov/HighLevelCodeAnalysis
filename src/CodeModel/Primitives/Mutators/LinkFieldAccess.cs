using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.Dependencies;
using Mono.Reflection;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.FieldAccessLinks)]
    [Need(Resources.Fields, Resources.Methods)]
    public class LinkFieldAccess : INodeMutator<MethodNode>
    {
        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (node.Method.GetMethodBody() == null)
            {
                return;
            }

            var instructions = node.Method.GetInstructions();

            foreach (var instruction in instructions.Where(x=>x.OpCode == OpCodes.Stfld))
            {
                var field = (FieldInfo) instruction.Operand;

                var targetNode = context.FindNodes<FieldNode>(x => x.Field == field).SingleOrDefault();

                if (targetNode != null)
                {
                    context.AddLink(node, targetNode, new SetFieldLink());
                }
            }

            foreach (var instruction in instructions.Where(x => x.OpCode == OpCodes.Ldfld))
            {
                var field = (FieldInfo)instruction.Operand;

                var targetNode = context.FindNodes<FieldNode>(x => x.Field == field).SingleOrDefault();

                if (targetNode != null)
                {
                    context.AddLink(node, targetNode, new GetFieldLink());
                }
            }
        }
    }
}