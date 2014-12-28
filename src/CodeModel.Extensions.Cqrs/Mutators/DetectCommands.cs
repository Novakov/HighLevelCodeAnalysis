using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
    [Provide(CqrsResources.Commands)]
    [Need(Resources.Types)]
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