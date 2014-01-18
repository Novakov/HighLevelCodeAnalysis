using System.Reflection;
using CodeModel.Builder;
using CodeModel.Model;

namespace CodeModel.Mutators
{
    public class AddMethods : INodeMutator<TypeNode>
    {
        public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;        

        public BindingFlags Flags { get; set; }

        public AddMethods()
        {
            this.Flags = DefaultFlags;
        }

        public AddMethods(BindingFlags flags)
        {
            this.Flags = flags;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var method in node.Type.GetMethods(Flags))
            {
                context.AddNode(new MethodNode(method));
            }
        }
    }
}