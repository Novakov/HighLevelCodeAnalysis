using System;
using System.Linq;
using CodeModel.Extensions.Cqrs;
using CodeModel.Links;
using CodeModel.Model;
using TestTarget.Cqrs;

namespace TestTarget.Conventions
{
    public class CqrsConvention : ICqrsConvention
    {
        public bool IsQuery(TypeNode node)
        {
            return node.Type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQuery<>));
        }

        public bool IsQueryExecution(MethodCallLink call)
        {
            throw new NotImplementedException();
        }

        public Type GetCalledQueryType(MethodCallLink call)
        {
            throw new NotImplementedException();
        }
    }
}
