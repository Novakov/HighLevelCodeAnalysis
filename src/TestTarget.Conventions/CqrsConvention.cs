﻿using System;
using System.Linq;
using System.Reflection;
using CodeModel.Extensions.Cqrs;
using CodeModel.FlowAnalysis;
using CodeModel.Primitives;
using TestTarget.Cqrs;

namespace TestTarget.Conventions
{
    public class CqrsConvention : ICqrsConvention
    {
        private static readonly MethodInfo QueryExecute = typeof (IQueryDispatcher).GetMethod("Query");
        private static readonly MethodInfo CommandExecute = typeof (ICommandDispatcher).GetMethod("Execute");

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
            return call.ActualParameterTypes[0][0].Type;
        }

        public bool IsCommandHandlerMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();

            return method.Name == "Execute"
                   && parameters.Length == 1
                   && typeof (ICommandHandler<>).MakeGenericType(parameters[0].ParameterType).IsAssignableFrom(method.DeclaringType);
        }

        public Type GetHandledCommand(MethodInfo method)
        {
            return method.GetParameters()[0].ParameterType;
        }

        public bool IsCommand(TypeNode node)
        {
            return typeof (ICommand).IsAssignableFrom(node.Type);
        }

        public bool IsCommandExecuteMethod(MethodInfo method)
        {
            return method == CommandExecute;
        }

        public Type GetExecutedCommandType(PotentialType[] actualParameterTypes)
        {
            return actualParameterTypes[0].Type;
        }
    }
}
