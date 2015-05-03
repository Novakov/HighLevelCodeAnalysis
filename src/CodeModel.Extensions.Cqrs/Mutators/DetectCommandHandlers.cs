using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
    [Need(Resources.Methods)]
    [Provide(CqrsResources.CommandHandlers)]
    public class DetectCommandHandlers : INodeMutator<MethodNode>
    {
        private readonly ICqrsConvention convention;

        public DetectCommandHandlers(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (node is CommandHandlerNode)
            {
                return;
            }

            if (this.convention.IsCommandHandlerMethod(node.Method))
            {
               context.ReplaceNode(node, new CommandHandlerNode(node.Method, convention.GetHandledCommand(node.Method))); 
            }
        }
    }
}