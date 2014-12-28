using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Conventions;
using CodeModel.FlowAnalysis;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs
{
    public interface ICqrsConvention : IConvention
    {
        bool IsQuery(TypeNode node);
        bool IsQueryExecution(MethodCallLink call);
        Type GetCalledQueryType(MethodCallLink call);
        bool IsCommandHandlerMethod(MethodInfo method);
        Type GetHandledCommand(MethodInfo method);
        bool IsCommand(TypeNode node);
        bool IsCommandExecuteMethod(MethodInfo method);
        Type GetExecutedCommandType(PotentialType[] actualParameterTypes);
    }
}
