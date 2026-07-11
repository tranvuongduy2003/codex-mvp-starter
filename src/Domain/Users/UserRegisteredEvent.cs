using Solution.Domain.Abstractions;

namespace Solution.Domain.Users;

public sealed record UserRegisteredEvent(
    UserId UserId,
    DisplayName DisplayName,
    EmailAddress Email) : DomainEvent;
