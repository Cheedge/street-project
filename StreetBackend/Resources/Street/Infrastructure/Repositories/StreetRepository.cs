using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StreetBackend.Resources.Street.Domain;
using StreetBackend.Resources.Street.Infrastructure.DbContexts;
using StreetBackend.Resources.Street.Infrastructure.Entities;

namespace StreetBackend.Resources.Street.Infrastructure.Repositories
{
	public class StreetRepository: IStreetRepository
	{
        private readonly StreetDbContext _context;
        private readonly ILogger<StreetRepository> _logger;
        private readonly IMapper _mapper;

        public StreetRepository(StreetDbContext context, ILogger<StreetRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<StreetDomain> GetByIdAsync(Guid id)
        {
            var entity = await _context.Streets.FindAsync(id)??
                throw new ArgumentNullException($"entity with {id} not exists");
            return _mapper.Map<StreetDomain>(entity);
        }

        public async Task<List<StreetDomain>> GetAllAsync()
        {
            var entities = await _context.Streets.ToListAsync();
            return entities.Select(e => _mapper.Map<StreetDomain>(e)).ToList();
        }

        public async Task AddAsync(StreetDomain street)
        {
            var entity = _mapper.Map<StreetEntity>(street);
            _context.Streets.Add(entity);
            await _context.SaveChangesAsync();
            street.SetRowVersion(entity.RowVersion);
        }

        public async Task UpdateAsync(StreetDomain street)
        {
            var entity = _mapper.Map<StreetEntity>(street);
            _context.Streets.Update(entity);
            await _context.SaveChangesAsync();
            street.SetRowVersion(entity.RowVersion);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Streets.FindAsync(id);
            if (entity != null)
            {
                _context.Streets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

