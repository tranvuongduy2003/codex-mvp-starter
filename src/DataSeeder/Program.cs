using System.Text.Json;
using Solution.DataSeeder.Helpers;
using Solution.DataSeeder.Seeders;
using Solution.Domain.Users;
using Solution.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

const string defaultPassword = "DevPass123!";

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

var connectionString = ConnectionStringResolver.Resolve();
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Connection string 'App' not found.");
    return 1;
}

var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
if (!Directory.Exists(dataDirectory))
{
    Console.Error.WriteLine($"Data directory not found: {dataDirectory}");
    return 1;
}

var options = new DbContextOptionsBuilder<ApplicationDatabaseContext>()
    .UseNpgsql(connectionString, npgsql =>
        npgsql.MigrationsHistoryTable("__ef_migrations_history", ApplicationDatabaseContext.SchemaName))
    .Options;

await using var dbContext = new ApplicationDatabaseContext(options);

Console.WriteLine("Applying migrations...");
await dbContext.Database.MigrateAsync();
Console.WriteLine("Migrations applied.");

var identityHasher = new PasswordHasher<object>();
var password = Password.Create(defaultPassword);
var hash = identityHasher.HashPassword(null!, password.Value);
var passwordHash = PasswordHash.Create(hash);
var now = DateTimeOffset.UtcNow;

await UserSeeder.SeedAsync(dbContext, passwordHash, now, dataDirectory, jsonOptions);

Console.WriteLine("Seeding complete.");
return 0;
