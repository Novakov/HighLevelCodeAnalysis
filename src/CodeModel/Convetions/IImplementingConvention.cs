using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Model;

namespace CodeModel.Convetions
{
    public interface IImplementingConvention : IConvention
    {
        bool ShouldInlineImplementationsFor(TypeNode interfaceNode);
    }
}
