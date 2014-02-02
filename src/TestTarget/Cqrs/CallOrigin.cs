using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget.Cqrs
{
    public class CallOrigin
    {
        public void CallQuery()
        {
            var queryDispatcher = GetQueryDispatcher();

            queryDispatcher.Query(new GetUser("aaaa"));
        }

        private static IQueryDispatcher GetQueryDispatcher()
        {
            return null;
        }
    }
}
