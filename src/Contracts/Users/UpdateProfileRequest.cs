namespace Solution.Contracts.Users;

public sealed record UpdateProfileRequest(string? DisplayName, string? Email);
