# Solution API instructions

## Scope

Applies to `src/Api/**`. Contract changes also require reading `src/Contracts/AGENTS.md`,
`contracts/AGENTS.md`, and `web/AGENTS.md`.

## Thin transport

- Minimal endpoints implement the existing `IEndpoint` convention and are discovered through assembly scanning.
- Endpoints bind, authenticate/authorize, dispatch to Application, and map results to HTTP only.
- Return explicit Contracts DTOs, never domain entities or EF models.
- Keep identifier shape from Contracts/OpenAPI: internal IDs are numeric; `format: uuid` is only a
  documented external/public identifier.
- Reuse existing result-to-response and ProblemDetails helpers.

## Security boundaries

- Browser auth uses secure, HttpOnly, SameSite-appropriate cookies and server-side session validation.
- Role-protected operations must use backend authorization policies.
- Never log passwords, cookies, session secrets, credentials, or sensitive uploaded content.

## Contract workflow

For a public REST shape change:

1. update the committed OpenAPI source through the repository workflow;
2. update Contracts and endpoint mapping;
3. regenerate/verify web API types;
4. add or update API integration tests.

## Verification

```powershell
yarn --cwd web api:verify
dotnet test Solution.slnx -c Release
```
