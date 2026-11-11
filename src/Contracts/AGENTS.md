# Solution Contracts instructions

## Scope

Applies to `src/Contracts/**`. Read `contracts/AGENTS.md` for public REST shape changes.

## Contract rules

- Keep request and response types explicit, transport-focused, and free of domain behavior.
- Do not expose domain entities, EF models, secrets, session identifiers, or infrastructure-specific types.
- Expose internal identifiers as numeric JSON (`integer`/`int64`). Use a GUID/UUID contract field
  only for a separately named external/public identifier intentionally exposed through a
  third-party API and marked `// id-policy: external-id`; never use it for an opaque internal ID.
- Keep names, nullability, formats, validation meaning, and enum values aligned with `contracts/openapi/api.v1.yaml`.
- Keep ProblemDetails codes and semantics stable and intentional.

## Change completion

A public contract change is incomplete until OpenAPI source, endpoint mapping, generated web types,
affected frontend calls, and integration tests agree.

## Verification

```powershell
yarn --cwd web api:verify
dotnet build Solution.slnx -c Release
dotnet test Solution.slnx -c Release
```
