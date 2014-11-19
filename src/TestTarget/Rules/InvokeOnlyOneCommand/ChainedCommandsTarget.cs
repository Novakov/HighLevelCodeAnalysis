using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTarget.Cqrs;

namespace TestTarget.Rules.InvokeOnlyOneCommand
{
    public class ChainedCommandsTarget
    {
        private readonly ICommandDispatcher commands;

        public ChainedCommandsTarget(ICommandDispatcher commands)
        {
            this.commands = commands;
        }

        public void PathWithThreeMethodsSecondAndThirdExecuteOneCommand()
        {
            this.OneCommandThanAnotherOne();
        }

        private void OneCommandThanAnotherOne()
        {
            this.commands.Execute(new RegisterUser());
            this.LastMethodThanExecutesOneCommand();
        }

        private void LastMethodThanExecutesOneCommand()
        {
            this.commands.Execute(new UnregisterUser());
        }
    }
}
