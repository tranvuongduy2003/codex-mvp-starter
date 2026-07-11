# Solution Domain instructions

## Scope

Applies to `src/Domain/**`.

## Domain purity

- Keep Domain pure C#.
- Do not reference EF Core, ASP.NET Core, MediatR, Redis, MinIO, Infrastructure, or transport contracts.
- Do not add persistence annotations, serialization concerns, dependency-injection code, logging, or HTTP concepts.

## Modeling rules

- Keep user behavior and value-object validation in Domain.
- Value objects are immutable, self-validating, and compared by value.
- Domain events describe facts that already occurred and stay mechanism-free.
- Use one term per concept across code, tests, contracts, and UI copy.

## Verification

Add or update focused domain unit tests for changed user behavior, validation, and state transitions.
