using Domain.Interfaces;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add Entity Framework
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var databaseProvider = configuration["DatabaseProvider"] ?? "SqlServer";

        services.AddDbContext<LibraryDbContext>(
            options =>
            {
                switch (databaseProvider.ToLower())
                {
                    case "sqlite":
                        options.UseSqlite(connectionString);
                        break;
                    case "sqlserver":
                    default:
                        options.UseSqlServer(connectionString);
                        break;
                }
            }
        );

        // Register repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBorrowerRepository, BorrowerRepository>();
        services.AddScoped<IBorrowingRepository, BorrowingRepository>();

        return services;
    }
}
