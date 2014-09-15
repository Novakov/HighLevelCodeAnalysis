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

        public void ExecuteCommand()
        {
            var commandDispatcher = GetCommandDispatcher();

            commandDispatcher.Execute(new RegisterUser());
        }

        public void ExecuteMultipleCommands()
        {
            var commandDispatcher = GetCommandDispatcher();
            commandDispatcher.Execute(new RegisterUser());
            commandDispatcher.Execute(new UnregisterUser());
        }

        private static ICommandDispatcher GetCommandDispatcher()
        {
            return null;
        }

        private static IQueryDispatcher GetQueryDispatcher()
        {
            return null;
        }
    }
}
