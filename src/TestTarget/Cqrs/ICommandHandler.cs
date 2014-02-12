﻿namespace TestTarget.Cqrs
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}