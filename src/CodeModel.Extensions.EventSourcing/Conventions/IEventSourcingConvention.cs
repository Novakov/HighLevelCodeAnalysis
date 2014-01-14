using System;
using CodeModel.Convetions;
using CodeModel.Links;

namespace CodeModel.Extensions.EventSourcing.Conventions
{
    public interface IEventSourcingConvention : IConvention
    {
        bool IsApplyEvent(MethodCallLink methodCallLink);

        Type ExtractEventType(MethodCallLink methodCallLink);
    }
}
