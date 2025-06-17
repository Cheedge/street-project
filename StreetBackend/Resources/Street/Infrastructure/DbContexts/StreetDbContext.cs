using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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
                .ToTable("Streets");

            modelBuilder.Entity<StreetEntity>()
                .HasKey(e => e.Id);

            // for optimistic concurrency
            // TODO: there is issue with pgcrypto extension, so temperaly use thiss
            modelBuilder.Entity<StreetEntity>()
                .Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                //.HasDefaultValueSql("gen_random_bytes(8)");
                //.ValueGeneratedOnAddOrUpdate();
                .ValueGeneratedNever();
        }
    }
}

