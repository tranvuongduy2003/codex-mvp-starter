using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Messaging;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Abstractions.Storage;
using Solution.Application.Common;

namespace Solution.Application.Users.Queries;

public sealed class GetCurrentUserQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUserRepository userRepository,
    IObjectStorage objectStorage)
    : QueryHandler<GetCurrentUserQuery, CurrentUserResult>
{
    private const string Bucket = "solution";

    public override async Task<Result<CurrentUserResult>> Handle(
        GetCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is null)
        {
            return Error.Unauthorized("UNAUTHORIZED", "Authentication is required.");
        }

        var user = await userRepository.GetByIdAsync(currentUserAccessor.UserId.Value, cancellationToken);
        if (user is null)
        {
            return Error.NotFound("USER_NOT_FOUND", "User was not found.");
        }

        var avatarUrl = user.AvatarImageRef is not null
            ? objectStorage.GetPublicUri(Bucket, user.AvatarImageRef.Value).ToString()
            : null;

        return new CurrentUserResult(
            user.Id.Value,
            user.DisplayName.Value,
            user.Email.DisplayValue,
            user.Role.ToString(),
            avatarUrl);
    }
}
