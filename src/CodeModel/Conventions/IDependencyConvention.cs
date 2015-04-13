using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Conventions
{
    public interface IDependencyConvention : IConvention
    {
        IEnumerable<Type> GetDependenciesForType(Type type);
    }
}
