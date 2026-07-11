# Solution backend testing instructions

## Scope

Applies to `tests/**` and backend test additions elsewhere.

## Test placement

- `tests/Domain.UnitTests`: user aggregate behavior, value objects, and state transitions.
- `tests/Api.IntegrationTests`: HTTP contracts, cookies/session, auth, EF mappings, PostgreSQL, and infrastructure boundaries.
- `tests/Testing.Common`: shared fixtures, fictional builders, assertions, fakes, clocks, and test infrastructure only.

## Rules

- Assert observable behavior and invariants, not private methods or implementation details.
- Mirror nearby fixture and assertion patterns before adding abstractions.
- Keep all data fictional; never use real credentials, tokens, user data, or production-like secrets.
- Cover validation and expected failure paths when request handling changes.
- Use integration tests for status codes, ProblemDetails, cookies, persistence, and authorization.
- Keep tests deterministic. Inject/control time; do not rely on wall-clock delays.

## Verification

```powershell
dotnet test Solution.slnx -c Release
```
