using System.Text.Json;
using Solution.DataSeeder.Models;
using Solution.Domain.Users;
using Solution.Infrastructure.Persistence;
using Solution.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Solution.DataSeeder.Seeders;

internal static class UserSeeder
{
    internal static async Task SeedAsync(
        ApplicationDatabaseContext dbContext,
        PasswordHash passwordHash,
        DateTimeOffset now,
        string dataDirectory,
        JsonSerializerOptions jsonOptions)
    {
        var filePath = Path.Combine(dataDirectory, "Users.json");
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"Users.json not found: {filePath}");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var seeds = JsonSerializer.Deserialize<List<UserSeed>>(json, jsonOptions);

        if (seeds is null or { Count: 0 })
        {
            Console.Error.WriteLine("Users.json is empty or invalid.");
            return;
        }

        var normalizedSeeds = seeds
            .Select(seed => new
            {
                seed.Id,
                seed.DisplayName,
                Email = seed.Email.ToLowerInvariant(),
                Role = Enum.Parse<UserRole>(seed.Role),
            })
            .ToList();

        var seedIds = normalizedSeeds.Select(seed => seed.Id).ToList();
        var seedEmails = normalizedSeeds.Select(seed => seed.Email).ToList();
        var existingUsers = await dbContext.Users
            .Where(user => seedIds.Contains(user.Id) || seedEmails.Contains(user.Email))
            .ToListAsync();

        var added = 0;
        var updated = 0;

        foreach (var seed in normalizedSeeds)
        {
            var existingUser = existingUsers.FirstOrDefault(user =>
                user.Id == seed.Id || user.Email == seed.Email);

            if (existingUser is null)
            {
                dbContext.Users.Add(new UserRecord
                {
                    Id = seed.Id,
                    DisplayName = seed.DisplayName,
                    Email = seed.Email,
                    PasswordHash = passwordHash.Value,
                    Role = seed.Role,
                    CreatedAt = now,
                    UpdatedAt = now,
                    RowVersion = 1,
                });
                added++;
                continue;
            }

            existingUser.DisplayName = seed.DisplayName;
            existingUser.Email = seed.Email;
            existingUser.PasswordHash = passwordHash.Value;
            existingUser.Role = seed.Role;
            existingUser.UpdatedAt = now;
            existingUser.RowVersion = Math.Max(existingUser.RowVersion + 1, 1);
            updated++;
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"Ensured {normalizedSeeds.Count} users ({added} added, {updated} updated).");
    }
}
