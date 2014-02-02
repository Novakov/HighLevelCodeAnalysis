using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Convetions;
using CodeModel.Links;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public interface ICqrsConvention : IConvention
    {
        bool IsQuery(TypeNode node);
        bool IsQueryExecution(MethodCallLink call);
        Type GetCalledQueryType(MethodCallLink call);
    }
}
