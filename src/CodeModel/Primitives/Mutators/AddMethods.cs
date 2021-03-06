using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.Methods)]
    [Need(Resources.Types)]
    public class AddMethods : INodeMutator<TypeNode>
    {
        public const BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public BindingFlags Flags { get; set; }

        public AddMethods()
            : this(DefaultFlags)
        {
        }

        public AddMethods(BindingFlags flags)
        {
            this.Flags = flags;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var method in node.Type.GetMethods(Flags).Where(x => !x.IsInherited()))
            {
                context.AddNode(new MethodNode(method));
            }
        }
    }
}