using System;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.CommandHandlers
{
	public class DeletePointFromStreetCommandHandler: ICommandHandler<DeletePointFromStreetCommand>
	{
        private readonly IStreetRepository _repository;
        private readonly ILogger<DeletePointFromStreetCommandHandler> _logger;

        public DeletePointFromStreetCommandHandler(
            IStreetRepository repository,
            ILogger<DeletePointFromStreetCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeletePointFromStreetCommand command)
        {
            var street = await _repository.GetByIdAsync(command.StreetId);
            var toRemove = new Coordinate(command.PointToRemove.X, command.PointToRemove.Y);
            var coords = street.Geometry.Geometry.Coordinates.Where(p => !(p.Y == toRemove.Y && p.X == toRemove.X)).ToList();
            street = StreetDomain.CreateStreet(street.Name, street.Capacity, (IEnumerable<Coordinate>)coords);
            await _repository.UpdateAsync(street);
        }
    }
}

