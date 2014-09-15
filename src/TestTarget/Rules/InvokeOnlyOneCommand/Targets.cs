using TestTarget.Cqrs;

namespace TestTarget.Rules.InvokeOnlyOneCommand
{
    public class Targets
    {
        private readonly ICommandDispatcher commands;

        public Targets(ICommandDispatcher commands)
        {
            this.commands = commands;
        }

        public void DispatchTwoCommands()
        {
            this.commands.Execute(new RegisterUser());
            this.commands.Execute(new UnregisterUser());
        }
    }
}
