using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.EventSourcing.Conventions;
using CodeModel.Extensions.EventSourcing.Links;
using CodeModel.Primitives;

namespace CodeModel.Extensions.EventSourcing.Mutators
{
    public class DetectApplyEvent : INodeMutator<MethodNode>
    {
        private readonly IEventSourcingConvention convention;

        public DetectApplyEvent(IEventSourcingConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            var applyLinks = node.OutboundLinks
                .OfType<MethodCallLink>()
                .Where(x => convention.IsApplyEvent(x))
                .ToList();

            foreach (var link in applyLinks)
            {
                var eventType = this.convention.ExtractEventType(link);

                var target = context.FindNodes<TypeNode>(x => x.Type == eventType).SingleOrDefault();

                if(target != null)
                { 
                    context.RemoveLink(link);
                    context.AddLink(node, target, new ApplyEventLink());
                }
            }
        }
    }
}
