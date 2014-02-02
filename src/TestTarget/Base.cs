using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class Base
    {
        public void Method()
        {
        }
    }

    public class Inherited : Base
    {
        public int Arg { get; set; }

        public Inherited()
        {            
        }

        public Inherited(int arg)
        {
            this.Arg = arg;
        }
    }

    public class SecondInherited : Base
    {
    }
}
