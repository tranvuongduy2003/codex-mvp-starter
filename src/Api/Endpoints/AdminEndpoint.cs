namespace Solution.Api.Endpoints;

internal sealed class AdminEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/api/admin/ping", () => Results.Ok(new AdminPingResponse("ok")))
            .WithName("AdminPing")
            .WithTags("Admin")
            .RequireAuthorization("AdminOnly")
            .Produces<AdminPingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
}

internal sealed record AdminPingResponse(string Status);
