using Solution.Application.Abstractions.Messaging;

namespace Solution.Application.Users.Commands;

public sealed record UploadAvatarCommand(
    Stream Content,
    string ContentType,
    string FileName) : ICommand<UploadAvatarResult>;

public sealed record UploadAvatarResult(string AvatarUrl);
