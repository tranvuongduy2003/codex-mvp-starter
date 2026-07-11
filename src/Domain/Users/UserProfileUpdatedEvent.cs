using Solution.Domain.Abstractions;

namespace Solution.Domain.Users;

public sealed record UserProfileUpdatedEvent(
    UserId UserId,
    DisplayName? DisplayName,
    EmailAddress? Email) : DomainEvent;
