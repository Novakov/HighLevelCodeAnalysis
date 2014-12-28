using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.Fields)]
    public class AddFields : INodeMutator<TypeNode>
    {
        public const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public BindingFlags Flags { get; set; }

        public AddFields()
            : this(DefaultFlags)
        {
        }

        public AddFields(BindingFlags flags = DefaultFlags)
        {
            this.Flags = flags;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var fieldInfo in node.Type.GetFields(Flags).Where(x => !x.IsCompilerGenerated()).Where(x => !x.IsInherited()))
            {
                context.AddNode(new FieldNode(fieldInfo));
            }
        }
    }
}