using System;
using AutoMapper;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.CommandHandlers
{
	public class PostPointToStreetCommandHandler: ICommandHandler<PostPointToStreetCommand>
	{
        private readonly IStreetRepository _repository;
        private readonly ILogger<PostPointToStreetCommandHandler> _logger;
        private readonly IMapper _mapper;

        public PostPointToStreetCommandHandler(
            IStreetRepository repository,
            ILogger<PostPointToStreetCommandHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task HandleAsync(PostPointToStreetCommand command)
        {
            var street = await _repository.GetByIdAsync(command.StreetId);
            var newPoint = new Coordinate(command.NewPoint.X, command.NewPoint.Y);
            if (street.IsCloserToStart(newPoint))
            {
                street.AddPointToStart(newPoint);
            }
            else
            {
                street.AddPointToEnd(newPoint);
            }
            await _repository.UpdateAsync(street);
        }
    }
}

