using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class LinkCalls
    {
        public void Source()
        {
            this.NormalCall();
            this.GenericMethodCall<int>();
        }

        public void GenericMethodCall<T>()
        {
            
        }

        public void NormalCall()
        {
            
        }
    }
}
