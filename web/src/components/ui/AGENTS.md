# UI primitive instructions

## Scope

Applies to `web/src/components/ui/**`. These files are the shared shadcn `base-nova`
primitives. Feature code must consume them; this directory is the only place where raw
interactive elements (`button`, `input`, `table`, `dialog`, etc.) are allowed.

Read `web/AGENTS.md` first. Read `web/src/components/ui/README.md` for the installed
groups and dependency notes.

## Editing rules

1. **Keep the upstream API.** Preserve exported names, composition parts, variants, and
   `render`-prop behavior. A rename or prop change is a breaking change for every feature
   and needs a migration in the same change.
2. **Keep upstream behavior.** Do not remove focus management, ARIA wiring, keyboard
   handling, portal behavior, or data attributes to make a local layout easier.
3. **Style through variants and tokens.** Add a variant when a new visual role is real and
   reusable. Do not hardcode colors, arbitrary sizes, or one-off spacing for a single
   feature caller.
4. **Use Base UI composition.** The primitives are Base UI based. Use the `render` prop
   for polymorphism; do not reintroduce Radix `asChild` patterns or duplicate a primitive
   with a different library.
5. **Reuse shared helpers.** `cn` comes from `@/lib/utils`; button styling comes from
   `./button-variants`; mobile detection comes from `@/hooks/use-mobile`.
6. **No product behavior.** Primitives stay presentational. Do not add API calls, query
   keys, auth rules, business validation, or product copy here.
7. **No eager barrel.** Do not add `index.ts` that re-exports every component. Direct
   module imports keep Recharts, Shiki, Embla, and `@shadcn/react` out of unrelated pages.
8. **`sonner.tsx` is the app Toaster adapter; `toast.tsx` is the Base UI primitive.**
   Application notifications use `toast()` from `sonner`. Do not mount both toasters.

## Update workflow

- Prefer the shadcn CLI/MCP with the project `components.json` (`base-nova`, `neutral`,
  Lucide) over hand-copying a component.
- After adding or changing a primitive, update `web/src/components/ui/README.md` if the
  component set changed.
- New production dependencies need explicit approval and a license review; record the
  purpose and license in the README's dependency table.

## Verification

```powershell
yarn --cwd web lint
yarn --cwd web build
```

Also exercise focus, keyboard, and overlay behavior in a browser for interaction changes
that TypeScript cannot prove.
