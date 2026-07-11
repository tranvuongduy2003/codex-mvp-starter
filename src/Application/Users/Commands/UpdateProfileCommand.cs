using Solution.Application.Abstractions.Messaging;

namespace Solution.Application.Users.Commands;

public sealed record UpdateProfileCommand(
    string? DisplayName,
    string? Email) : ICommand<UpdateProfileResult>;

public sealed record UpdateProfileResult(
    Guid UserId,
    string DisplayName,
    string Email,
    string? AvatarUrl);
