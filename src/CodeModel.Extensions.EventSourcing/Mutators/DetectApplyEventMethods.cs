using CodeModel.Builder;
using CodeModel.Extensions.EventSourcing.Conventions;
using CodeModel.Extensions.EventSourcing.Nodes;
using CodeModel.Graphs;
using CodeModel.Primitives;

namespace CodeModel.Extensions.EventSourcing.Mutators
{
    public class DetectApplyEventMethods : INodeMutator<MethodNode>
    {
        private readonly IEventSourcingConvention convention;

        public DetectApplyEventMethods(IEventSourcingConvention convention)
        {
            this.convention = convention;
        }

        public void Mutate(MethodNode node, IMutateContext context)
        {
            if (!(node is ApplyEventMethod) && convention.IsApplyEventMethod(node))
            {
                context.ReplaceNode(node, new ApplyEventMethod(node, convention.GetEventAppliedByMethod(node)));
            }   
        }
    }
}