using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Conventions;
using CodeModel.Primitives;

namespace TestTarget.Conventions
{
    public class ImplementingConvention : IImplementingConvention
    {
        public bool ShouldInlineImplementationsFor(TypeNode interfaceNode)
        {
            return true;
        }
    }
}
