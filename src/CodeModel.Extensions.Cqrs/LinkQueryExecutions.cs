using System.Linq;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class LinkQueryExecutions : INodeMutator<MethodNode>
    {
        private readonly ICqrsConvention convention;

        public LinkQueryExecutions(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            foreach (var call in node.OutboundLinks.OfType<MethodCallLink>().ToList())
            {
                if (convention.IsQueryExecution(call))
                {
                    context.RemoveLink(call);

                    var queryType = convention.GetCalledQueryType(call);

                    var query = context.FindNodes<QueryNode>(n => n.Type == queryType).SingleOrDefault();
                    if (query != null)
                    {
                        context.AddLink(node, query, new QueryExecutionLink());
                    }
                }
            }
        }
    }
}