# Solution Application instructions

## Scope

Applies to `src/Application/**`.

## Ownership

Application owns commands, queries, handlers, validators, pipeline behaviors, application services,
repository/external-system ports, and current-user access.

## CQRS and flow

- Commands implement `ICommand` / `ICommand<T>` and may mutate state.
- Queries implement `IQuery<T>` and must be side-effect free.
- Handlers return `Result` / `Result<T>` for expected outcomes.
- Validate at the application boundary before state mutation.
- Use `ICurrentUserAccessor` for authenticated user context.
- Use `IClock` for time-dependent behavior.
- Pass numeric `int`/`long` IDs or ID value objects across ports. Do not introduce `Guid` IDs for new
  aggregates; the shipped auth `Guid` shape is baseline migration debt.
- Keep handlers orchestration-focused.

## Pipeline contract

Do not casually reorder or bypass the MediatR pipeline:

1. logging/correlation;
2. validation;
3. post-commit session-cache update;
4. unit of work and bounded optimistic-concurrency retry;
5. in-process domain-event dispatch before commit;
6. handler.

## Verification

Use application or API integration coverage for validation, session orchestration, authorization, and
port interactions.
