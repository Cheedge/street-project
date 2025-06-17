using System;
using System.Data;
using System.Security.Cryptography;
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
            // TODO: new version is redundent here, as in my domain
            //       already set a new version. but some issue as new
            //       version not change, need to fix
            var newVersion = RandomNumberGenerator.GetBytes(8);
            street.SetRowVersion(newVersion);
            var entity = _mapper.Map<StreetEntity>(street);
            _context.Streets.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update the street points (Obsolete!)
        /// </summary>
        /// <param name="street"></param>
        /// <returns></returns>
        public async Task UpdateAsync(StreetDomain street)
        {
            var entity = _mapper.Map<StreetEntity>(street);
            _context.Streets.Update(entity);
            await _context.SaveChangesAsync();
            street.SetRowVersion(entity.RowVersion);
        }

        public async Task UpdateStreetByAlgoAsync(StreetDomain street)
        {
            //var newVersion = RandomNumberGenerator.GetBytes(8);
            //street.SetRowVersion(newVersion);
            var entity = _mapper.Map<StreetEntity>(street);

            var trackedEntity = _context.Streets.Local.FirstOrDefault(e => e.Id == street.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Streets.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                street.SetRowVersion(entity.RowVersion);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DBConcurrencyException("The street was modified by another user.");
            }
        }

        public async Task UpdateStreetByStoredProcedualAsync(StreetDomain street, Coordinate coord, bool isCloserToStart)
        {
            try
            {
                // TODO: need to use pgcrypto, but failed, for now just passing in the
                //       row version as parameter.
                var newVersion = RandomNumberGenerator.GetBytes(8);
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"SELECT add_point_to_street({street.Id}, {coord.X}, {coord.Y}, {isCloserToStart}, {street.RowVersion}, {newVersion})"
                );
                street.SetRowVersion(newVersion);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Concurrency conflict") == true)
            {
                throw new DBConcurrencyException("The street was modified by another user.");
            }
        }

        public async Task DeleteStreetAsync(Guid id)
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

