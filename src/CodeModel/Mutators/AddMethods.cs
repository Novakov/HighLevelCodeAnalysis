using System.Reflection;
using CodeModel.Builder;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class AddMethods : INodeMutator<TypeNode>
    {
        public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        private readonly BindingFlags flags;

        public AddMethods(BindingFlags flags = DefaultFlags)
        {
            this.flags = flags;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var method in node.Type.GetMethods(flags))
            {
                context.AddNode(new MethodNode(method));
            }
        }
    }
}