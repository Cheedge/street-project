using System;
using StreetBackend.Resources.Street.Domain;

namespace StreetBackend.Resources.Street.Infrastructure.Repositories
{
	public interface IStreetRepository
	{
        Task<StreetDomain> GetByIdAsync(Guid id);
        Task<List<StreetDomain>> GetAllAsync();
        Task AddAsync(StreetDomain street);
        Task UpdateAsync(StreetDomain street);
        Task UpdateStreetByAlgoAsync(StreetDomain street);
        Task UpdateStreetByStoredProcedualAsync(StreetDomain street, Coordinate coord, bool isCloserToStart);
        Task DeleteStreetAsync(Guid id);
    }
}

