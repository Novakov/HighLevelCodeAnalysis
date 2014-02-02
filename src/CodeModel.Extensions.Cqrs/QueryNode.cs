using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class QueryNode : TypeNode
    {
        public QueryNode(Type type) : base(type)
        {
        }
    }
}
