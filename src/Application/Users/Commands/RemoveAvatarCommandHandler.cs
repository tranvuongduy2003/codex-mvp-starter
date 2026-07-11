using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Messaging;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Abstractions.Services;
using Solution.Application.Abstractions.Storage;
using Solution.Application.Common;
using Solution.Domain.Users;

namespace Solution.Application.Users.Commands;

public sealed class RemoveAvatarCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUserRepository userRepository,
    IObjectStorage objectStorage,
    IPendingDomainEventsCollector pendingDomainEventsCollector,
    IPendingSessionCacheCollector pendingSessionCacheCollector)
    : CommandHandler<RemoveAvatarCommand>
{
    private const string Bucket = "solution";

    public override async Task<Result> Handle(
        RemoveAvatarCommand command,
        CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Error.Unauthorized("UNAUTHORIZED", "You must be signed in.");
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Error.NotFound("USER_NOT_FOUND", "User not found.");
        }

        if (user.AvatarImageRef is null)
        {
            return Result.Success();
        }

        await objectStorage.DeleteAsync(Bucket, user.AvatarImageRef.Value, cancellationToken);

        user.RemoveAvatar();

        pendingDomainEventsCollector.AddRange(user.DomainEvents);
        user.ClearDomainEvents();

        pendingSessionCacheCollector.Enqueue(
            new PendingSessionCacheEntry(
                Guid.Empty,
                user.Id.Value,
                DateTimeOffset.MinValue));

        return Result.Success();
    }
}
