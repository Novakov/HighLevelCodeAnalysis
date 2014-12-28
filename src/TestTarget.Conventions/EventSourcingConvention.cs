using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.EventSourcing.Conventions;
using CodeModel.Primitives;
using TestTarget.EventSourcing;

namespace TestTarget.Conventions
{
    public class EventSourcingConvention : IEventSourcingConvention
    {
        private static readonly MethodInfo ApplyMethod = typeof (EntityBase).GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic);

        public bool IsApplyEvent(MethodCallLink methodCallLink)
        {
            return ((MethodNode) methodCallLink.Target).Method == ApplyMethod;
        }

        public Type ExtractEventType(MethodCallLink methodCallLink)
        {
            return methodCallLink.GenericMethodArguments[0];
        }

        public bool IsApplyEventMethod(MethodNode node)
        {
            var parameters = node.Method.GetParameters();
            return !node.Method.IsStatic && node.Method.Name == "On" && parameters.Length == 1 && typeof (DomainEvent).IsAssignableFrom(parameters[0].ParameterType);
        }

        public Type GetEventAppliedByMethod(MethodNode node)
        {
            return node.Method.GetParameters()[0].ParameterType;
        }
    }
}
