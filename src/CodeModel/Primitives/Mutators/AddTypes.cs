using CodeModel.Builder;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Provide(Resources.Types)]
    public class AddTypes : INodeMutator<AssemblyNode>
    {
        public void Mutate(AssemblyNode node, IMutateContext context)
        {
            var types = node.Assembly.GetTypes();

            foreach (var type in types)
            {
                context.AddNode(new TypeNode(type));
            }
        }
    }
}