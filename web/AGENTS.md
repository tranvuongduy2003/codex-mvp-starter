# Solution web instructions

## Scope

Applies to `web/**`. Read `contracts/AGENTS.md` when a frontend change requires a public API shape change.

## Structure and data flow

- Keep router, providers, route paths, and guards under `web/src/app`.
- Keep feature-specific UI and data access inside `web/src/features`.
- Reuse primitives from `web/src/components/ui`.
- Use generated API types from `web/src/generated`; never duplicate server contracts by hand.
- Use TanStack Query for server state.
- Use Zustand only for appropriate client/session/UI state.
- Frontend guards improve navigation/UX only; backend authorization remains mandatory.

## UX rules

- Keep auth forms typed, accessible, keyboard-usable, validated, and clear about server/ProblemDetails errors.
- Use semantic HTML and existing accessible UI primitives.
- Keep the starter shell simple and product-neutral.

## Generated API output

- Do not edit `web/src/generated/**` directly.
- Change `contracts/openapi/api.v1.yaml`, update runtime contracts, and run the generation/verification workflow instead.

## Verification

```powershell
yarn --cwd web api:verify
yarn --cwd web lint
yarn --cwd web format:check
yarn --cwd web build
```
