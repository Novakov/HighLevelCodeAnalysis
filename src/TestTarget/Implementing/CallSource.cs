using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget.Implementing
{
    public class CallSource
    {
        public void Call(IInterface target)
        {
            target.ImplicitMethod();
        }
    }
}
