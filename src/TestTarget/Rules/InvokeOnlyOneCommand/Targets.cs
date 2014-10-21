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

        public void DispatchOneCommandInSingleBranch()
        {
            
        }

        public void NoCommandDispatch()
        {
            this.commands.Execute(new RegisterUser());
        }


        public void DispatchTwoCommandOnePerBranch()
        {
            if (Get<bool>())
            {
                this.commands.Execute(new RegisterUser());
            }
            else
            {
                this.commands.Execute(new UnregisterUser());
            }
        }

        public static T Get<T>()
        {
            return default(T);
        }
    }
}
