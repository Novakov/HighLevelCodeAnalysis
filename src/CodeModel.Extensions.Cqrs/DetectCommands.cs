using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class DetectCommands : INodeMutator<TypeNode>
    {
        private readonly ICqrsConvention convention;

        public DetectCommands(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (node is CommandNode)
            {
                return;
            }

            if (convention.IsCommand(node))
            {
                context.ReplaceNode(node, new CommandNode(node.Type));
            }
        }
    }
}