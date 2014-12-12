using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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

        private void AnotherMethodExecutingOneCommand()
        {
            this.commands.Execute(new RegisterUser());
        }

        public void TwoBranchesOneWithSingleCommandSecondWithTwoCommands()
        {
            if (Get<bool>())
            {
                this.PathWithThreeMethodsSecondAndThirdExecuteOneCommand();
            }
            else
            {
                this.LastMethodThanExecutesOneCommand();
            }
        }

        public void TwoBranchesEachWithOneCommand()
        {
            if (Get<bool>())
            {
                this.LastMethodThanExecutesOneCommand();
            }
            else
            {
                this.AnotherMethodExecutingOneCommand();
            }
        }

        private T Get<T>()
        {
            return default(T);
        }
    }
}
