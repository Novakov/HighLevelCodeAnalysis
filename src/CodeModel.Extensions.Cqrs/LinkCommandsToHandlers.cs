using System.Linq;
using CodeModel.Builder;

namespace CodeModel.Extensions.Cqrs
{
    public class LinkCommandsToHandlers : INodeMutator<CommandHandlerNode>
    {
        public void Mutate(CommandHandlerNode node, IMutateContext context)
        {
            var commandNode = context.FindNodes<CommandNode>(x => x.Type == node.HandledCommand).SingleOrDefault();

            if (commandNode != null)
            {
                context.AddLink(commandNode, node, new ExecutedByLink());
            }
        }
    }
}