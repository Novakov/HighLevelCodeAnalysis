﻿using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs.Mutators
{
    [Need(Resources.Types)]
    [Provide(CqrsResources.Queries)]
    public class DetectQueries : INodeMutator<TypeNode>
    {
        private readonly ICqrsConvention convention;

        public DetectQueries(ICqrsConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            if (node is QueryNode)
            {
                return;
            }

            if (this.convention.IsQuery(node))
            {
                context.ReplaceNode(node, new QueryNode(node.Type));
            }
        }
    }
}
