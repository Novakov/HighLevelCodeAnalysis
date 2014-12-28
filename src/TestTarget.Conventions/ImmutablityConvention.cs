using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Conventions;
using CodeModel.Primitives;

namespace TestTarget.Conventions
{
    public class ImmutablityConvention : IImmutablityConvention
    {
        public bool IsImmutableType(TypeNode node)
        {
            return node.Type.GetCustomAttribute<ImmutableAttribute>() != null;
        }
    }
}
