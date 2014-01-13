using System.Linq;
using CodeModel.Builder;
using CodeModel.Model;

namespace CodeModel.Mutators
{
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