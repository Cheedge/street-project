using System;
namespace StreetBackend.Common.Interfaces
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}

