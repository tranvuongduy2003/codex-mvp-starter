using System.Net;
using System.Net.Http.Json;
using Solution.Api.IntegrationTests.Integration;
using Solution.Application.Abstractions.Auth;
using Solution.Contracts.Users;
using Solution.Domain.Users;
using Solution.Infrastructure.Persistence;
using Solution.Infrastructure.Persistence.Entities;
using Solution.Testing.Common.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Solution.Api.IntegrationTests.Users;

[Collection(IntegrationTestCollection.Name)]
public sealed class AdminAuthorizationTests(IntegrationTestFixture fixture)
{
    private HttpClient CreateClient() =>
        fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = true });

    [Fact]
    public async Task AdminEndpoint_WithoutSession_Returns401()
    {
        using var client = CreateClient();

        using var response = await client.GetAsync("/api/admin/ping");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AdminEndpoint_WithRegularUser_Returns403()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        using var client = CreateClient();

        using var registerResponse = await client.PostAsJsonAsync(
            "/api/users",
            new RegisterUserRequest($"user_{suffix}", $"user_{suffix}@example.com", "SecurePass1!"));

        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        using var response = await client.GetAsync("/api/admin/ping");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AdminEndpoint_WithAdminUser_Returns200()
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var email = $"admin_{suffix}@example.com";
        const string password = "SecurePass1!";

        await SeedUserAsync($"admin_{suffix}", email, password, UserRole.Admin);

        using var client = CreateClient();
        using var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginUserRequest(email, password));

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        using var response = await client.GetAsync("/api/admin/ping");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task SeedUserAsync(
        string displayName,
        string email,
        string password,
        UserRole role)
    {
        await using var scope = fixture.Factory.Services.CreateAsyncScope();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var databaseContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        var now = DateTimeOffset.UtcNow;

        var passwordHash = passwordHasher.Hash(Password.Create(password));
        databaseContext.Users.Add(new UserRecord
        {
            Id = Guid.NewGuid(),
            DisplayName = displayName,
            Email = EmailAddress.Create(email).Value,
            PasswordHash = passwordHash.Value,
            Role = role,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = 1,
        });

        await databaseContext.SaveChangesAsync();
    }
}
