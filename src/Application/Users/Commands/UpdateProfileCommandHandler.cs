using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Messaging;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Abstractions.Services;
using Solution.Application.Abstractions.Storage;
using Solution.Application.Common;
using Solution.Domain.Exceptions;
using Solution.Domain.Users;

namespace Solution.Application.Users.Commands;

public sealed class UpdateProfileCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUserRepository userRepository,
    IObjectStorage objectStorage,
    IPendingDomainEventsCollector pendingDomainEventsCollector,
    IPendingSessionCacheCollector pendingSessionCacheCollector)
    : CommandHandler<UpdateProfileCommand, UpdateProfileResult>
{
    private const string Bucket = "solution";

    public override async Task<Result<UpdateProfileResult>> Handle(
        UpdateProfileCommand command,
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

        if (command.Email is not null)
        {
            var email = EmailAddress.Create(command.Email);
            if (email.Value != user.Email.Value)
            {
                if (await userRepository.ExistsByEmailExcludingUserIdAsync(email.Value, userId, cancellationToken))
                {
                    return ProfileErrors.EmailTaken;
                }
            }
        }

        try
        {
            DisplayName? displayName = command.DisplayName is not null
                ? DisplayName.Create(command.DisplayName)
                : null;

            EmailAddress? email = command.Email is not null
                ? EmailAddress.Create(command.Email)
                : null;

            user.UpdateProfile(displayName, email);

            pendingDomainEventsCollector.AddRange(user.DomainEvents);
            user.ClearDomainEvents();

            pendingSessionCacheCollector.Enqueue(
                new PendingSessionCacheEntry(
                    Guid.Empty,
                    user.Id.Value,
                    DateTimeOffset.MinValue));

            var avatarUrl = user.AvatarImageRef is not null
                ? objectStorage.GetPublicUri(Bucket, user.AvatarImageRef.Value).ToString()
                : null;

            return new UpdateProfileResult(
                user.Id.Value,
                user.DisplayName.Value,
                user.Email.DisplayValue,
                avatarUrl);
        }
        catch (BusinessRuleValidationException exception)
        {
            return Error.Validation(
                exception.Code ?? Error.ValidationFailedCode,
                exception.Message);
        }
    }
}
