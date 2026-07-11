# Solution Infrastructure instructions

## Scope

Applies to `src/Infrastructure/**`.

## Adapter boundaries

- Implement Application ports for EF Core, user repositories, cache/session, object storage, and password hashing.
- Infrastructure may reference Application and Domain; it must not reference Api.
- PostgreSQL schema `app` is authoritative.
- Redis is disposable/rebuildable cache and session infrastructure.
- Default EF queries to no tracking unless mutation or identity resolution requires tracking.
- Keep avatar relational state to object keys and metadata; do not store bytes or expiring presigned URLs.

## EF Core migrations

- Existing shared migrations are immutable once published.
- Generate new migrations with EF tooling and review them before keeping them.
- Make required fields, max lengths, indexes, foreign keys, delete behavior, defaults, concurrency tokens, and schema names explicit.
- Update integration tests when persistence behavior changes.

## Verification

```powershell
dotnet build Solution.slnx -c Release
dotnet test Solution.slnx -c Release
```
