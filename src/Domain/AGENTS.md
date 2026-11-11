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
- Entity/aggregate IDs and their value objects wrap `int` or `long`, never `Guid`. A GUID is only a
  separately named external/public identifier for third-party exposure.
- The shipped `UserId` Guid key is baseline migration debt; do not add another Guid ID type.
- Domain events describe facts that already occurred and stay mechanism-free.
- Use one term per concept across code, tests, contracts, and UI copy.

## Verification

Add or update focused domain unit tests for changed user behavior, validation, and state transitions.
