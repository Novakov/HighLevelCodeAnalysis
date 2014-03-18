using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Convetions;
using CodeModel.Model;

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
