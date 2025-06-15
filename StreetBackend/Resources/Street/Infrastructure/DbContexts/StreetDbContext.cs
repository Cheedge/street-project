using System;
using Microsoft.EntityFrameworkCore;
using StreetBackend.Resources.Street.Infrastructure.Entities;

namespace StreetBackend.Resources.Street.Infrastructure.DbContexts
{
	public class StreetDbContext: DbContext
	{
        public DbSet<StreetEntity> Streets { get; set; }

        public StreetDbContext(DbContextOptions<StreetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StreetEntity>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<StreetEntity>()
                .Property(e => e.RowVersion)
                .IsRowVersion(); // for optimistic concurrency
        }
    }
}

