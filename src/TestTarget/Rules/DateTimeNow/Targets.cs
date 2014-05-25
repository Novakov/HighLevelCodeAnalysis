using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget.Rules.DateTimeNow
{
    public class Targets
    {
        public void UseDateTimeNow()
        {
            Console.WriteLine(DateTime.Now);
        }

        public void UseDateTimeUtcNow()
        {
            Console.WriteLine(DateTime.UtcNow);
        }

        public void UseDateTimeOffsetNow()
        {
            Console.WriteLine(DateTimeOffset.Now);
        }

        public void UseDateTimeOffsetUtcNow()
        {
            Console.WriteLine(DateTimeOffset.UtcNow);
        }

        public void DoNotUseDateTimeNow()
        {
            
        }
    }
}
