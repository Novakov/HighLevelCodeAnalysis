using System.Linq;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
    [Provide(CqrsResources.CommandExecutionLinks)]
    [Need(Resources.Methods, CqrsResources.Commands)]
    public class LinkCommandExecutions : INodeMutator<MethodNode>
    {
        private readonly ICqrsConvention convention;

        public LinkCommandExecutions(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (convention.IsCommandExecuteMethod(node.Method))
            {
                foreach (var call in node.InboundLinks.OfType<MethodCallLink>().ToList())
                {
                    foreach (var types in call.ActualParameterTypes)
                    {
                        var commandType = this.convention.GetExecutedCommandType(types);

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
}