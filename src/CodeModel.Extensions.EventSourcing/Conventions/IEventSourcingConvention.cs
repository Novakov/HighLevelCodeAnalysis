using System;
using CodeModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.EventSourcing.Conventions
{
    public interface IEventSourcingConvention : IConvention
    {
        bool IsApplyEvent(MethodCallLink methodCallLink);

        Type ExtractEventType(MethodCallLink methodCallLink);
        bool IsApplyEventMethod(MethodNode node);
        Type GetEventAppliedByMethod(MethodNode node);
    }
}
