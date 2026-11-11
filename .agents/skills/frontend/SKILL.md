---
name: frontend
description: "Source-driven frontend workflow for Solution web/ work: React 19 + Vite + TypeScript routes, feature modules, auth/session UX, account/profile workflows, OpenAPI client usage, TanStack Query, Zustand, the complete base-nova shadcn component set, component-first composition, Tailwind v4 styling, and frontend verification. Use before editing or reviewing web/**."
---

# Frontend Skill

Use this skill to work from the current `web/` source code. Treat `AGENTS.md` files as rules/policy; keep this skill procedural and do not copy rules back into it.

## Boundary

- Rules live in root `AGENTS.md`, `web/AGENTS.md`, `web/src/components/ui/AGENTS.md`,
  `web/src/generated/AGENTS.md`, and the three specs under `docs/`.
- This skill only tells Codex how to scout, edit, and verify frontend work.
- If a rule and this skill disagree, follow the rule/spec and report the drift in the skill.

## Frontend Entry Pass

1. Read root `AGENTS.md`, then `web/AGENTS.md`. Read `web/src/generated/AGENTS.md` only when generated API output is involved.
2. Read the relevant `docs/features.md` acceptance criteria for user-visible behavior. Add `docs/product.md` or `docs/technical.md` only when product guardrails, architecture, security, authorization, or integration behavior is at stake.
3. Discover the actual source before deciding an approach:

```powershell
rg --files web/src
rg "<domain term>|route|queryKey|mutationFn|schema|ProblemDetails|ApiError" web/src
```

4. Read nearby files in the owning feature before editing: route file, page, API module, components, schema/types, tests/e2e if present.
5. Choose only the reference files needed for the task.
6. Run the component-first pass below before writing JSX.

## Component-First Pass

The repo ships the complete shadcn `base-nova` set. Do not hand-roll UI.

1. List every UI affordance the change needs: action, input, choice, overlay, navigation,
   data display, feedback, layout surface.
2. Map each affordance to a `web/src/components/ui` primitive using the table in
   `web/AGENTS.md` or `$component-first`.
3. Read the chosen primitive's signature before use; do not guess props or variants.
4. Compose full groups (`Card` family, `Field` family, `Table` family, `Dialog` family).
5. Only use semantic HTML for landmarks, prose, links, forms, and content images.
6. If no primitive fits, stop and justify one of: compose existing primitives, add a
   primitive through `$shadcn`, or use a documented semantic-HTML exception. Never fall
   back to a raw control.

## Source Map

Use the current codebase as the source of structure:

- `web/src/app`: router, providers, path constants, route guard composition.
- `web/src/features`: workflow-owned modules shaped as `api.ts`, `schemas.ts`,
  `errors.ts`, `query-keys.ts`, `components/`, `hooks/`, `pages/`, and `routes.tsx`;
  add `index.ts` only when another module imports the feature.
- `web/src/components/ui`: complete shadcn `base-nova` primitives; the only place raw controls are allowed.
- `web/src/layouts`: shared page shells.
- `web/src/lib/api`: API client, interceptors, and OpenAPI type helpers.
- `web/src/hooks`: shared hooks, including `use-mobile` and `use-highlighted-code`.
- `web/src/store`: small client/session state stores.
- `web/src/styles`: Tailwind v4 globals, shadcn variants, and design-system utilities.
- `web/src/types`: reusable frontend schemas/types.
- `web/src/generated`: generated OpenAPI types; read-only.

## Reference Routing

- Route, module, layout, or file placement changes: read `references/architecture.md`.
- UI primitive or component composition changes: read `references/components.md`; load
  `$component-first` and use `$shadcn` when primitives or `components.json` are involved.
- API calls, server state, cache invalidation, or error surfaces: read `references/data-fetching.md`; use `$openapi-contract-sync` when API shape changes.
- Forms, validation, or server error mapping: read `references/forms.md`.
- Zustand or session/client state: read `references/state.md`; use `$zustand-web` for store design.
- Tailwind v4, tokens, responsive layout, or visual polish: read `references/styling.md`; use `$tailwind-patterns` for deeper Tailwind v4 work.
- Browser verification, screenshots, console, or network behavior: use `$chrome-devtools`/browser skill after implementation.

## Working Loop

1. Restate the frontend scope, touched feature/module, applicable rules/spec sections, and expected verification.
2. Build from existing source patterns instead of inventing a parallel pattern.
3. Compose shadcn primitives for every affordance; keep raw semantic HTML to the allowlist.
4. Keep edits local to the owning feature unless shared behavior is genuinely required.
5. Make user-facing states complete for the changed workflow: loading, empty, error, pending mutation, success/navigation, and mobile layout.
6. For contract drift, change the OpenAPI/backend contract through the owning workflow; do not patch generated frontend types by hand.
7. Before handoff, run the narrowest useful checks, then broaden when the change crosses shared app, API, or critical browser workflow boundaries.

## Source-Driven Checks

Before editing, answer from source:

- Which feature owns the workflow?
- Which existing route/page/API/helper already does the closest thing?
- Which `web/src/components/ui` primitive owns each affordance?
- Which generated contract type or local response type is currently used?
- Which cache keys, store fields, or navigation paths are affected?
- Which acceptance criteria or product guardrails does the user-visible behavior touch?

## References

- `references/architecture.md`
- `references/components.md`
- `references/data-fetching.md`
- `references/forms.md`
- `references/state.md`
- `references/styling.md`

## Verification

Pick commands from the actual change surface:

```powershell
yarn --cwd web api:verify
yarn --cwd web lint
yarn --cwd web format:check
yarn --cwd web build
```

Before handoff on changed-code work, prefer the repo verifier:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/agent/Verify-ChangedCode.ps1
```
