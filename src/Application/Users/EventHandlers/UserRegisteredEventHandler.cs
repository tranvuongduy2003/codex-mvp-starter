using Solution.Application.Abstractions.Messaging;
using Solution.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Solution.Application.Users.EventHandlers;

internal sealed class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    : IDomainEventHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "UserRegistered {UserId} {DisplayName}",
            domainEvent.UserId.Value,
            domainEvent.DisplayName.Value);

        return Task.CompletedTask;
    }
}
