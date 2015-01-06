using System.Collections.Generic;
using System.Linq;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    [Need(DomainModelResources.ContainedEntitiesLink)]
    public class AvoidBidirectionalReferenceRule : INodeRule
    {
        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            var containers = node.InboundLinks.OfType<ReferenceLink>().Select(x => x.Source).Distinct();
            var backReferences = containers.SelectMany(x => x.OutboundLinks).OfType<ReferenceLink>()
                .Where(x => x.Target == node);

            foreach (var backReference in backReferences)
            {
                yield return new BidirectionalReferenceViolation(backReference.Source, backReference.Target);
            }
        }

        public bool IsApplicableTo(Node node)
        {
            return node is EntityNode;
        }
    }
}