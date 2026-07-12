using System.Threading.Channels;
using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Cache;
using Solution.Application.Abstractions.IntegrationEvents;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Abstractions.Storage;
using Solution.Infrastructure.Auth;
using Solution.Infrastructure.Cache;
using Solution.Infrastructure.IntegrationEvents;
using Solution.Infrastructure.Persistence;
using Solution.Infrastructure.Persistence.Repositories;
using Solution.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Solution.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("App");
        var redisConnectionString = configuration.GetConnectionString("Cache");
        var storageConnectionString = configuration.GetConnectionString("storage");

        void ConfigureApplicationDatabaseContext(DbContextOptionsBuilder contextOptions)
        {
            contextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            if (!string.IsNullOrWhiteSpace(postgresConnectionString))
            {
                contextOptions.UseNpgsql(
                    postgresConnectionString,
                    npgsql => npgsql.MigrationsHistoryTable(
                        "__ef_migrations_history",
                        ApplicationDatabaseContext.SchemaName));
            }
        }

        services.AddDbContext<ApplicationDatabaseContext>(ConfigureApplicationDatabaseContext);
        services.AddDbContextFactory<ApplicationDatabaseContext>(
            ConfigureApplicationDatabaseContext,
            ServiceLifetime.Scoped);

        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'Cache' is required. Configure Redis for session cache.");
        }

        if (string.IsNullOrWhiteSpace(storageConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'storage' is required. Configure object storage for user avatars.");
        }

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnectionString));
        services.AddSingleton<ICacheService, CacheService>();

        services.AddScoped<IApplicationDatabaseContext>(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationDatabaseContext>());
        services.AddScoped<IApplicationDatabaseContextFactory, ApplicationDatabaseContextFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionStore, SessionStore>();
        services.AddScoped<IPasswordHasher, IdentityPasswordHasher>();

        services.AddSingleton<IObjectStorage>(_ => new MinioObjectStorage(storageConnectionString));
        services.AddSingleton(_ => Channel.CreateUnbounded<QueuedIntegrationEvent>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            }));
        services.AddSingleton<IIntegrationEventPublisher, ChannelIntegrationEventPublisher>();
        services.AddHostedService<ChannelIntegrationEventConsumerService>();
        return services;
    }
}
