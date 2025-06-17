using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.CommandHandlers
{
	public class DeleteStreetCommandHandler: ICommandHandler<DeleteStreetCommand>
	{
        private readonly IStreetRepository _repository;
        private readonly ILogger<DeleteStreetCommandHandler> _logger;

        public DeleteStreetCommandHandler(
            IStreetRepository repository,
            ILogger<DeleteStreetCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteStreetCommand command)
        {
            await _repository.DeleteStreetAsync(command.StreetId);
        }
    }
}

