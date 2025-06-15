using System;
using AutoMapper;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Application.Queries;
using StreetBackend.Resources.Street.Infrastructure.Repositories;

namespace StreetBackend.Resources.Street.Application.QueryHandlers
{
    public class GetStreetByFieldQueryHandler : IQueryHandler<GetStreetByFieldQuery, StreetDto>
    {
        private readonly IStreetRepository _repository;
        private readonly ILogger<GetStreetByFieldQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetStreetByFieldQueryHandler(
            IStreetRepository repository,
            ILogger<GetStreetByFieldQueryHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<StreetDto> HandleAsync(GetStreetByFieldQuery query)
        {
            var street = await _repository.GetByIdAsync(query.Id);
            return _mapper.Map<StreetDto>(street);
        }
    }
}

