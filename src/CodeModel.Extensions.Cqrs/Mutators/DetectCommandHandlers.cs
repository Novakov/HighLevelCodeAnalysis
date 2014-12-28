using CodeModel.Builder;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
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

            if (convention.IsCommandHandlerMethod(node.Method))
            {
               context.ReplaceNode(node, new CommandHandlerNode(node.Method, convention.GetHandledCommand(node.Method))); 
            }
        }
    }
}