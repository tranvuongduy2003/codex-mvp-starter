---
doc_schema: solution-doc-v1
doc_kind: source_spec
doc_id: solution.technical
title: Solution Technical Skeleton
status: template
version: "1.0"
last_updated: 2026-07-11
owner: builder
language: en
authority: architecture_domain_model_data_integrations_operations_and_verification
source_documents:
  - docs/product.md
  - docs/features.md
---

# Solution Technical Skeleton (`technical.md`)

This file is the technical template for the boilerplate. Replace or extend it as product-specific
architecture emerges.

## Architecture

Solution uses Clean Architecture, Domain-Driven Design, and CQRS:

- `Domain` contains pure C# user entities, value objects, and domain events.
- `Application` contains commands, queries, validators, behaviors, and ports.
- `Infrastructure` implements EF Core persistence, session/cache, password hashing, and storage.
- `Api` is the composition root and HTTP transport boundary.
- `Contracts` contains explicit request and response DTOs.
- `contracts/openapi/api.v1.yaml` is the committed REST contract source.

## Runtime

Aspire is the local topology source of truth. The starter includes:

- PostgreSQL as authoritative persistence.
- Redis as rebuildable session/cache infrastructure.
- MinIO-compatible object storage for user avatars.
- OpenTelemetry through ServiceDefaults.
- In-memory integration events use `Channel<QueuedIntegrationEvent>` with typed consumers resolved
  through dependency injection.
- React, TypeScript, Vite, and Yarn for the web app.

## Domain Model

The starter keeps only the user/account domain:

- `User`
- `UserId`
- `EmailAddress`
- `DisplayName`
- `Password`
- `PasswordHash`
- `AvatarImageRef`
- `UserRole` with `User` and `Admin`

## Security

- Browser authentication uses secure, HttpOnly cookies.
- Session validation is server-side.
- Role authorization is enforced by backend policies.
- Passwords are stored only as approved hashes.
- Logs and errors must not contain passwords, cookies, session identifiers, or secrets.

## Verification Strategy

Use the lowest useful level first:

- Domain unit tests for value objects and user aggregate behavior.
- API integration tests for contracts, cookies, sessions, persistence, and authorization.
- Playwright e2e tests for register, login, logout, and route guards.
- Frontend build/lint/format checks for the app shell.

## Feature Completion

A feature is complete when `features.md`, OpenAPI, backend contracts, frontend usage, tests, and
CI agree. Product-specific behavior must be added to this template before implementation.
