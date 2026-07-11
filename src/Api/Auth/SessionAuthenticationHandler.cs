using System.Security.Claims;
using System.Text.Encodings.Web;
using Solution.Application.Abstractions.Auth;
using Solution.Application.Abstractions.Persistence;
using Solution.Application.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Solution.Api.Auth;

internal sealed class SessionAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISessionStore sessionStore,
    IUserRepository userRepository,
    IOptions<AuthSessionOptions> sessionOptions) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Cookies.TryGetValue(sessionOptions.Value.CookieName, out var sessionIdValue)
            || !Guid.TryParse(sessionIdValue, out var sessionId))
        {
            return AuthenticateResult.NoResult();
        }

        var userId = await sessionStore.ResolveUserIdAsync(sessionId, Context.RequestAborted);
        if (userId is null)
        {
            return AuthenticateResult.Fail("Session is invalid or expired.");
        }

        var user = await userRepository.GetByIdAsync(userId.Value, Context.RequestAborted);
        if (user is null)
        {
            return AuthenticateResult.Fail("Session user was not found.");
        }

        var claims = new[]
        {
            new Claim(SessionAuthenticationDefaults.UserIdClaimType, userId.Value.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
