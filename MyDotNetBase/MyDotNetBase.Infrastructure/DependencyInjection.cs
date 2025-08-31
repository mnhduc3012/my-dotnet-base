using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Roles.Enums;
using MyDotNetBase.Domain.Shared.Constants;
using MyDotNetBase.Domain.Users.Services;
using MyDotNetBase.Infrastructure.Identity;
using MyDotNetBase.Infrastructure.Outbox;
using MyDotNetBase.Infrastructure.Persistence;
using MyDotNetBase.Infrastructure.Persistence.Interceptors;
using MyDotNetBase.Infrastructure.Persistence.Repositories;
using MyDotNetBase.Infrastructure.Services;
using Quartz;
using Quartz.Simpl;
using System.Text;

namespace MyDotNetBase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabaseServices(configuration)
            .AddIdentityServices(configuration)
            .AddDomainServices()
            .AddBackgroundJobsServices();

        return services;
    }

    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        services.AddScoped<ISaveChangesInterceptor, AuditSaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, ConvertDomainEventsToOutboxMessageInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention()
                   .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IStoredProcedureExecutor, DapperStoredProcedureExecutor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));

        services.AddSingleton<ITokenProvider, JwtTokenProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthorization(options =>
        {
            foreach (var permission in Enum.GetNames(typeof(Permission)))
            {
                options.AddPolicy(permission, policy =>
                    policy.RequireClaim(CustomClaimTypes.Permission, permission));
            }
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                };
            });

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailUniquenessChecker, UserService>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobsServices(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever())
                )
                .UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
