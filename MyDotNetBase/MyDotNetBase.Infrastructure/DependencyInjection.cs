using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Users.Services;
using MyDotNetBase.Infrastructure.Identity;
using MyDotNetBase.Infrastructure.Persistence;
using MyDotNetBase.Infrastructure.Persistence.Repositories;
using MyDotNetBase.Infrastructure.Services;

namespace MyDotNetBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);

        services.AddIdentityServices();

        services.AddDomainServices();

        return services;
    }

    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IStoredProcedureExecutor, DapperStoredProcedureExecutor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailUniquenessChecker, UserService>();

        return services;
    }
}
