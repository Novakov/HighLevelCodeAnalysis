using TestTarget.Cqrs;

namespace TestTarget.Rules.UnusedCommands
{
    public class Targets
    {
        private readonly ICommandDispatcher commands;

        public Targets(ICommandDispatcher commands)
        {
            this.commands = commands;
        }

        public void CallTwoCommands()
        {
            this.CallRegisterUser();
            this.CallUnregisterUser();
        }

        public void CallOnlyOneCommand()
        {
            this.CallRegisterUser();
        }

        public void CallRegisterUser()
        {
            this.commands.Execute(new RegisterUser());
        }

        public void CallUnregisterUser()
        {
            this.commands.Execute(new UnregisterUser());
        }
    }
}