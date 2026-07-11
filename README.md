# Solution

Solution is a reusable application boilerplate. It uses a .NET Clean Architecture + CQRS + DDD
backend, a React + TypeScript + Vite frontend, and .NET Aspire for the local topology.

## About

The starter keeps only the cross-product foundation: users, authentication, authorization, session
persistence, profile/avatar support, OpenAPI sync, frontend route guards, e2e auth journeys, scripts,
and CI.

## Codex Agent Setup

| Piece | Purpose |
| --- | --- |
| `AGENTS.md` and nested `AGENTS.md` files | Repository and path-scoped working rules |
| `.agents/skills/` | Reusable workflows for backend, frontend, E2E, OpenAPI, MCP, and harness work |
| `.codex/agents/` | Focused subagent prompts for review, security, acceptance, implementation, and harness diagnosis |
| `.codex/hooks.json` and `.codex/hooks/` | Guard, formatting, telemetry, and stop-verification hooks |
| `scripts/agent/` | Current validation sensors: changed-code, docs, and harness policy |

Agents should read the smallest relevant set of `docs/product.md`, `docs/features.md`, and
`docs/technical.md`, then the applicable `AGENTS.md` files and skill instructions before changing
code.

## Stack Highlights

- Modular monolith with Clean Architecture boundaries.
- PostgreSQL is authoritative; Redis is rebuildable cache/session infrastructure.
- MinIO-compatible object storage supports user avatars.
- React 19 + Vite frontend consumes generated OpenAPI types.
- .NET Aspire AppHost is the local topology source of truth. Do not add hand-authored Docker Compose.

## Prerequisites

- .NET 10 SDK
- Aspire CLI 13.3+
- Node.js 22 LTS and Yarn
- Docker Desktop or a compatible container runtime

## First-Time Setup

From the repository root:

```powershell
.\scripts\Setup-Environments.ps1
```

Or manually:

```powershell
dotnet restore Solution.slnx
yarn --cwd web install
dotnet dev-certs https
dotnet dev-certs https --trust
```

Create local `.env` files from the examples when needed. Do not commit secrets or machine-specific
environment files.

## Run Locally

```powershell
dotnet run --project src/AppHost/Solution.AppHost.csproj
```

Aspire exposes the API, web app, PostgreSQL, Redis, MinIO, and seeder resources through the dashboard.

## Seed Accounts

The data seeder creates fictional development users with password `DevPass123!`:

| Email | Role |
| --- | --- |
| `admin@solution.dev` | Admin |
| `user@solution.dev` | User |

## Docs

| Document | Role |
| --- | --- |
| `docs/product.md` | Product intent and scope skeleton |
| `docs/features.md` | Feature and acceptance skeleton |
| `docs/technical.md` | Architecture and verification skeleton |
| `docs/specs/` | Implementation-spec template index |
| `docs/harness/` | Codex harness operating notes |

## Common Verification

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/agent/Verify-ChangedCode.ps1
dotnet build Solution.slnx -c Release
dotnet test Solution.slnx -c Release
yarn --cwd web api:verify
yarn --cwd web build
yarn --cwd e2e test
```

## API Contract

```powershell
yarn --cwd web api:export
yarn --cwd web api:codegen
```

See `contracts/openapi/README.md`.
