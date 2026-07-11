using Solution.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Solution.Infrastructure.Persistence;

internal sealed class ApplicationDatabaseContextFactory(
    IDbContextFactory<ApplicationDatabaseContext> databaseContextFactory)
    : IApplicationDatabaseContextFactory
{
    public async Task<IApplicationDatabaseContext> CreateApplicationDatabaseContextAsync(
        CancellationToken cancellationToken = default) =>
        await databaseContextFactory.CreateDbContextAsync(cancellationToken);
}
