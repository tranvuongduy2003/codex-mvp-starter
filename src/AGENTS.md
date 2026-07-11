# Solution backend instructions

## Scope

Applies to all code under `src/**`. Read the nearest child `AGENTS.md` before editing a specialized layer.

## Required context

- Read `docs/technical.md` for architecture, persistence, API, security, and verification rules.
- Read `docs/features.md` before changing observable auth/user behavior.
- Read `docs/product.md` before changing scope.

## Backend boundaries

- Preserve `Domain <- Application <- Infrastructure`; `Api` is the composition root.
- Keep Domain pure and behavioral.
- Keep use-case orchestration and ports in Application.
- Keep EF Core, cache/session, password hashing, and object storage adapters in Infrastructure.
- Keep HTTP/auth/session transport in Api.
- Return explicit Contracts DTOs from endpoints, never domain entities or EF records.
- Use `Result`/`Error` for expected failures.
- Inject `IClock`; do not read system time directly in domain or application logic.
- Keep sensitive values out of logs, errors, comments, fixtures, and telemetry.
- Enforce protected operations on the backend. Frontend guards are not authorization.

## Correctness priorities

Treat these as high risk whenever affected:

- password validation and hashing;
- session creation, revocation, expiry, and cache invalidation;
- cookie security settings;
- role claims and role-protected policies;
- unique email enforcement;
- profile/avatar object keys and public URI handling.

## Local runtime

- `.NET Aspire` AppHost is the local topology source of truth.
- Do not add hand-authored Docker Compose.
- PostgreSQL is authoritative.
- Redis is rebuildable cache/session infrastructure.
- MinIO-compatible storage stores avatar objects by key.
- Configure shared telemetry, service discovery, resilience, and health through `ServiceDefaults`.

## Verification

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/agent/Verify-ChangedCode.ps1
dotnet build Solution.slnx -c Release
dotnet test Solution.slnx -c Release
```
