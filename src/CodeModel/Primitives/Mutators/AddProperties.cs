using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.Properties)]
    public class AddProperties : INodeMutator<TypeNode>
    {        
        public const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        public BindingFlags Flags { get; set; }

        public AddProperties() 
            : this(DefaultFlags)
        {
        }

        public AddProperties(BindingFlags flags)
        {
            this.Flags = flags;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            foreach (var propertyInfo in node.Type.GetProperties().Where(x => !x.IsInherited()))
            {
                context.AddNode(new PropertyNode(propertyInfo));
            }
        }
    }
}