using System;
using System.Linq;
using System.Reflection;
using CodeModel.Extensions.Cqrs;
using CodeModel.Links;
using CodeModel.Model;
using TestTarget.Cqrs;

namespace TestTarget.Conventions
{
    public class CqrsConvention : ICqrsConvention
    {
        private static readonly MethodInfo QueryExecute = typeof (IQueryDispatcher).GetMethod("Query");

        public bool IsQuery(TypeNode node)
        {
            return node.Type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQuery<>));
        }

        public bool IsQueryExecution(MethodCallLink call)
        {
            return ((MethodNode) call.Target).Method.IsGenericMethod && ((MethodNode) call.Target).Method == QueryExecute;
        }

        public Type GetCalledQueryType(MethodCallLink call)
        {
            throw new NotImplementedException();
        }
    }
}
