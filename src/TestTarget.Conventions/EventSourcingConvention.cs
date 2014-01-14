using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.EventSourcing.Conventions;
using CodeModel.Links;
using CodeModel.Model;
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
    }
}
