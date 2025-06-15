using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.CommandHandlers
{
	public class CreateNewStreetCommandHandler: ICommandHandler<CreateNewStreetCommand>
	{
        private readonly IStreetRepository _repository;
        private readonly ILogger<CreateNewStreetCommandHandler> _logger;

        public CreateNewStreetCommandHandler(
            IStreetRepository repository,
            ILogger<CreateNewStreetCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(CreateNewStreetCommand command)
        {
            var points = command.Coordinates.Select(c => new Coordinate(c.Y, c.X));
            var street = StreetDomain.CreateStreet(command.Name, command.Capacity, points);
            await _repository.AddAsync(street);
        }
    }
}

