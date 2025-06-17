using System;
using Microsoft.EntityFrameworkCore;
using StreetBackend.Resources.Street.Infrastructure.DbContexts;

namespace StreetBackend
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StreetDbContext>();

            await context.Database.MigrateAsync();

            var sqlPath = Path.Combine(AppContext.BaseDirectory, "SqlScripts", "AddPointToStreet.sql");
            if (File.Exists(sqlPath))
            {
                var sql = await File.ReadAllTextAsync(sqlPath);
                await context.Database.ExecuteSqlRawAsync(sql);
            }
        }
    }

}

