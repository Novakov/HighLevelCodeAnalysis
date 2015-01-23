using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    public class DoNotReferenceAggregateDirectlyRule : INodeRule, IRuleWithBootstrap
    {
        private List<AggregateNode> aggregates;

        public void Initialize(VerificationContext context, Graph graph)
        {
            this.aggregates = graph.Nodes.OfType<AggregateNode>().ToList();
        }

        public bool IsApplicableTo(Node node)
        {
            return node is PropertyNode
                   && (node.GetContainer() is AggregateNode || node.GetContainer() is EntityNode);
        }

        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            var property = (PropertyNode) node;

            var referencedAggregate = this.aggregates.FirstOrDefault(x => x.Type == property.Property.PropertyType);

            if (referencedAggregate != null)
            {
                yield return new DirectAggregateReferenceViolation(node, referencedAggregate);
            }
        }
    }
}