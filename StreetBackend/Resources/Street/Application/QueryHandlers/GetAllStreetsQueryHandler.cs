using System;
using AutoMapper;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Application.Queries;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.QueryHandlers
{
	public class GetAllStreetsQueryHandler : IQueryHandler<GetAllStreetsQuery, List<StreetDto>>
    {
        private readonly IStreetRepository _repository;
        private readonly ILogger<GetAllStreetsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllStreetsQueryHandler(
            IStreetRepository repository,
            ILogger<GetAllStreetsQueryHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<StreetDto>> HandleAsync(GetAllStreetsQuery query)
        {
            var streets = await _repository.GetAllAsync();
            return streets.Select(s=>_mapper.Map<StreetDto>(s)).ToList();
        }
    }
}

