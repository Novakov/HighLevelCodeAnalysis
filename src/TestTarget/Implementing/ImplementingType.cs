using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget.Implementing
{
    public class ImplementingType : IInterface
    {
        public void ImplicitMethod()
        {
            throw new NotImplementedException();
        }
    }

    public interface IInterface
    {
        void ImplicitMethod();
    }
}
