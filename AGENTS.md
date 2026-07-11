# SOLUTION - Codex project instructions

Solution is a Clean Architecture + CQRS + DDD boilerplate. Local topology is .NET Aspire only.

Backend topology:

- `.NET` Minimal API composition root in `Api`
- Clean Architecture layers: `Domain`, `Application`, `Infrastructure`, `Contracts`
- MediatR command/query flows in `Application`
- EF Core persistence in `Infrastructure`
- `ServiceDefaults` shared by local services
- PostgreSQL as authoritative persistence
- Redis as rebuildable session/cache infrastructure
- MinIO-compatible object storage for user avatars

Frontend topology:

- React + TypeScript + Vite
- Yarn
- Tailwind/shadcn-style primitives where applicable
- Generated OpenAPI client/types consumed from the contract pipeline

## Source of truth

Keep the skeleton docs as templates:

| Document | Authority |
|---|---|
| `docs/product.md` | Product intent and scope template |
| `docs/features.md` | Observable behavior and acceptance template |
| `docs/technical.md` | Architecture, data, security, and verification template |

These docs are intentionally generic. Do not reintroduce product-specific business behavior unless the
docs are updated first.

## Non-negotiables

1. Architecture flows `Domain -> Application -> Infrastructure`; `Api` is the composition root.
2. `Solution.Domain` is pure C#: no EF Core, ASP.NET, MediatR, Infrastructure references,
   persistence annotations, or transport concerns.
3. Commands and queries stay separated in `Application` and flow through MediatR.
4. API endpoints are thin. They map HTTP to application use cases and return `Contracts` DTOs,
   never domain entities.
5. OpenAPI shape lives in `contracts/openapi/api.v1.yaml`; do not hand-edit generated web API output.
6. Aspire AppHost is the local topology source of truth. Do not add hand-authored Docker Compose.
7. `Solution.ServiceDefaults` is mandatory for `Api`.
8. Protected operations must be enforced on the backend. Frontend guards are UX support only.
9. Do not add production dependencies without explicit approval and a license check.

## Repository layout

```text
src/       AppHost, ServiceDefaults, Api, Application, Domain, Infrastructure, Contracts, DataSeeder
tests/     Domain.UnitTests, Api.IntegrationTests, Testing.Common
e2e/       Playwright e2e tests
web/       React + Vite frontend; Yarn; run through Aspire web resource
docs/      skeleton source specs and implementation-spec template index
contracts/ OpenAPI contract and codegen scripts
.codex/    Codex config, hooks, local state, custom agents
.agents/   Repo-local skills and workflow procedures
scripts/   Stable agent and verification scripts
```

## Nested AGENTS routing

Before editing a scoped area, read the nearest matching `AGENTS.md` even when the current session was
started at the repository root.

| Instructions | Applies to |
|---|---|
| `src/AGENTS.md` | All backend and host code under `src/**` |
| `src/Domain/AGENTS.md` | User aggregate, value objects, and domain events |
| `src/Application/AGENTS.md` | Commands, queries, handlers, validators, behaviors, ports |
| `src/Infrastructure/AGENTS.md` | EF Core, repositories, migrations, cache, storage |
| `src/Api/AGENTS.md` | Endpoints, HTTP mapping, auth/session transport, ProblemDetails |
| `src/Contracts/AGENTS.md` | Explicit request/response contract types |
| `contracts/AGENTS.md` | Committed OpenAPI source and contract-generation workflow |
| `tests/AGENTS.md` | Domain unit, API integration, and shared fixtures |
| `web/AGENTS.md` | React/Vite frontend, forms, server/client state, accessibility, route guards |
| `web/src/generated/AGENTS.md` | Generated API output; source-only changes are forbidden here |
| `e2e/AGENTS.md` | Playwright auth/session journeys |

## Matching skills

Use the relevant skill first for scoped work:

| Trigger | Skill |
|---|---|
| Full idea/spec/task loop | `$cook` |
| Backend, EF, endpoint, auth/session, backend test | `$backend` |
| Frontend route/component/form/styling | `$frontend` |
| Playwright browser journeys | `$e2e` |
| shadcn UI primitives / `components.json` | `$shadcn` |
| Zustand stores | `$zustand-web` |
| Library/framework/API docs | `$context7-research` |
| OpenAPI contract sync | `$openapi-contract-sync` |
| Open a PR handoff | `$create-pr` |

## Build and test

Default changed-code verification:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/agent/Verify-ChangedCode.ps1
```

Common direct checks:

```powershell
dotnet build Solution.slnx -c Release
dotnet test Solution.slnx -c Release
yarn --cwd web api:verify
yarn --cwd web build
yarn --cwd e2e test
```

## GitHub operations

Opening, updating, or merging PRs requires explicit human approval. Use the configured GitHub MCP
surface for repository metadata, issues, pull requests, checks, and reviews when available.

## Current scope

The boilerplate keeps only users, authentication, authorization, session persistence, profile/avatar,
the protected frontend shell, auth e2e journeys, OpenAPI sync, Aspire, scripts, and CI.

## Do not touch

Do not edit, regenerate, delete, or commit these unless the owning workflow explicitly requires it
and approval/verification is clear:

- `.env`, `.env.*`, `.mcp.json`, local secret files, private keys, credential files, `secrets/`
- `web/src/generated/`
- generated OpenAPI build output
- generated files such as `*.g.ts`, `*.generated.*`

Additional project rules:

- Do not run destructive git operations.
- Do not use npm in `web/`; use Yarn.
- Do not hand-author Docker Compose for local service topology.
- Do not log or document secrets, cookies, tokens, passwords, raw uploaded content, or sensitive user
  data.
