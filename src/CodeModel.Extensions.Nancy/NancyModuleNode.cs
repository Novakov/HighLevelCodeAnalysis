using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Nancy
{
    public class NancyModuleNode : TypeNode
    {
        public NancyModuleNode(Type type) : base(type)
        {
        }
    }
}
