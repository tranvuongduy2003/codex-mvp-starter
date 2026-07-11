using Solution.Domain.Exceptions;
using Solution.Domain.Users;
using FluentAssertions;

namespace Solution.Domain.UnitTests.Users;

public class UserRegisterTests
{
    private static readonly DateTimeOffset RegisteredAt = new(2026, 5, 23, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Register_WithValidInput_CreatesUser()
    {
        var user = RegisterUser(UserRole.User);

        user.Id.Value.Should().NotBe(Guid.Empty);
        user.DisplayName.Value.Should().Be("Jane User");
        user.Email.Value.Should().Be("jane@example.com");
        user.Role.Should().Be(UserRole.User);
    }

    [Fact]
    public void Register_WithAdminRole_CreatesAdminUser()
    {
        var user = RegisterUser(UserRole.Admin, "Admin User", "admin@example.com");

        user.Id.Value.Should().NotBe(Guid.Empty);
        user.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public void Register_WithInvalidDisplayName_Throws()
    {
        var act = () => DisplayName.Create("   ");

        act.Should()
            .Throw<BusinessRuleValidationException>()
            .Which.Code.Should().Be("DISPLAY_NAME_REQUIRED");
    }

    [Fact]
    public void Register_RaisesUserRegisteredEvent()
    {
        var user = RegisterUser(UserRole.User);

        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserRegisteredEvent>()
            .Which.Should().BeEquivalentTo(
                new UserRegisteredEvent(user.Id, user.DisplayName, user.Email),
                options => options.Excluding(domainEvent => domainEvent.OccurredOn));
    }

    [Fact]
    public void Register_SetsCorrectIdAndTimestamps()
    {
        var user = RegisterUser(UserRole.User);

        user.Id.Value.Should().NotBe(Guid.Empty);
        user.CreatedAt.Should().Be(RegisteredAt);
        user.UpdatedAt.Should().Be(RegisteredAt);
    }

    private static User RegisterUser(
        UserRole role,
        string displayName = "Jane User",
        string email = "Jane@Example.com") =>
        User.Register(
            DisplayName.Create(displayName),
            EmailAddress.Create(email),
            PasswordHash.Create("hashed-password-stub"),
            role,
            RegisteredAt);
}
