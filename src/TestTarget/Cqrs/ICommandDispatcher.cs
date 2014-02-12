namespace TestTarget.Cqrs
{
    public interface ICommandDispatcher
    {
        void Execute(ICommand command);
    }
}