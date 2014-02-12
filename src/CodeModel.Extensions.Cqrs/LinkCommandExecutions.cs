using System.Linq;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class LinkCommandExecutions : INodeMutator<MethodNode>
    {
        private readonly ICqrsConvention convention;

        public LinkCommandExecutions(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (convention.IsCommandExecuteMethod(node))
            {
                foreach (var call in node.InboundLinks.OfType<MethodCallLink>().ToList())
                {
                    var commandType = this.convention.GetExecutedCommandType(call);

                    var commandNode = context.FindNodes<CommandNode>(x => x.Type == commandType).SingleOrDefault();

                    if (commandNode != null)
                    {
                        context.AddLink(call.Source, commandNode, new ExecuteCommandLink());
                        context.RemoveLink(call);
                    }
                }
            }
        }
    }
}