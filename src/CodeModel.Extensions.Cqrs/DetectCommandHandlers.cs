using CodeModel.Builder;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
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

            if (convention.IsCommandHandlerMethod(node))
            {
               context.ReplaceNode(node, new CommandHandlerNode(node.Method, convention.GetHandledCommand(node.Method))); 
            }
        }
    }
}