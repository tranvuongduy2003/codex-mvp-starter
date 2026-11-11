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

## Feature module anatomy

Mirror the auth feature when adding or reorganizing a feature:

```text
features/<feature>/
  api.ts            transport calls and generated API types
  schemas.ts        Zod schemas and inferred form types
  errors.ts         ProblemDetails -> field/root/UI messages
  query-keys.ts     centralized query key factory
  components/       feature UI; add index.ts when other modules consume it
  hooks/            feature hooks; add index.ts when other modules consume it
  pages/            route-level pages
  routes.tsx        RouteObject[] with lazy page imports
  index.ts          public feature API, only when app/hooks/other features import it
```

Conventions:

- Feature-owned types live in that feature's `schemas.ts`; cross-feature types stay in
  `web/src/types`. `types/auth.ts` re-exports the auth schemas for compatibility.
- Route files lazy-load pages (`lazy: async () => { const { X } = await import(...) }`)
  so pages stay out of the initial bundle.
- Import through the feature barrel (`@/features/auth`) for `authApi`, `authQueryKeys`,
  `authRoutes`, and shared components/hooks; use relative imports inside the feature.
- Do not use inline query key arrays when the feature owns a `query-keys.ts` factory.

## Component-first UI contract (mandatory)

The starter ships the complete official shadcn `base-nova` set in `web/src/components/ui`.
Agents must **compose these components**, not recreate their behavior with hand-written
semantic HTML. A raw `<button>`, `<input>`, `<table>`, or `<dialog>` in feature code is a
policy violation, even when it looks shorter.

1. **Never render a raw interactive control.** Use the primitive that owns the behavior,
   accessibility, and styling.
2. **Compose full component groups.** A card is `Card` + `CardHeader` + `CardTitle` +
   `CardDescription` + `CardContent` + `CardFooter`, not a styled `<div>`.
3. **Allowed semantic HTML is limited to document structure, prose, links, forms, and images.**
   - Landmarks/layout: `div`, `span`, `main`, `header`, `footer`, `section`, `article`, `aside`, `form`.
   - Prose: `h1`-`h6`, `p`, `ul`, `ol`, `li`, `blockquote`, `pre`, `code`, `strong`, `em`.
   - Links: `a` / React Router `Link` for navigational links; use `Button render={<Link />}`
     when the affordance is an action.
   - Media: `img` for content images; `AvatarImage` for avatars; `AspectRatio` for ratios.
4. **No parallel primitives.** Do not add a second button, input, modal, toast, table, or
   icon library. Extend the installed primitive instead.
5. **Local wrappers stay thin.** Feature components compose primitives; they must not
   reimplement variants, focus management, or ARIA wiring.

### Need-to-primitive map

| Need                          | Compose                                                                                    |
| ----------------------------- | ------------------------------------------------------------------------------------------ |
| Action / submit / link-button | `Button`, `ButtonGroup`                                                                    |
| Text field                    | `Input`, `Textarea`, `Field`, `FieldLabel`, `FieldDescription`, `FieldError`, `FieldGroup` |
| Choice (native)               | `Select` or `NativeSelect`, `Combobox`                                                     |
| Choice (toggle)               | `Checkbox`, `RadioGroup`, `Switch`, `Slider`, `Toggle`, `ToggleGroup`                      |
| Date / time                   | `Calendar` + `Popover`                                                                     |
| OTP / code                    | `InputOTP`                                                                                 |
| Page surface                  | `Card` family, `Separator`, `ScrollArea`, `Resizable`                                      |
| Primary navigation            | `NavigationMenu`, `Breadcrumb`, `Sidebar`                                                  |
| Menus / commands              | `DropdownMenu`, `ContextMenu`, `Menubar`, `Command`                                        |
| Overlay / dialog              | `Dialog`, `AlertDialog`, `Sheet`, `Drawer`, `Popover`, `HoverCard`, `Tooltip`              |
| Disclosure                    | `Accordion`, `Collapsible`, `Tabs`                                                         |
| Data table                    | `Table` family                                                                             |
| Status / feedback             | `Alert`, `Badge`, `Progress`, `Spinner`, `Skeleton`, `Empty`, `Sonner` (`toast`)           |
| Identity                      | `Avatar` family, `Badge`                                                                   |
| List rows / metadata          | `Item` family, `Kbd`, `Marker`                                                             |
| Charts                        | `Chart` family + Recharts primitives                                                       |
| Pagination                    | `Pagination`                                                                               |
| Carousel / media              | `Carousel`, `AspectRatio`                                                                  |
| Code / stream content         | `CodeBlock`, `Message`, `MessageScroller`, `Bubble`, `Trace`, `Attachment`                 |

Read `web/src/components/ui/README.md` for the installed groups and
`web/src/components/ui/AGENTS.md` before editing a primitive.

### Banned raw elements outside `web/src/components/ui/**`

`button`, `input`, `select`, `option`, `optgroup`, `textarea`, `label`, `fieldset`,
`legend`, `progress`, `meter`, `output`, `table`, `thead`, `tbody`, `tfoot`, `tr`, `th`,
`td`, `caption`, `dialog`, `details`, `summary`, `hr`, `nav`.

This list is enforced by ESLint (`no-restricted-syntax`). Do not add file-level disables
without a written justification in the PR/handoff.

### Composition and styling rules

1. **Prefer variants and sizes over local classes.** `Button variant="outline"`, not a
   new button style. Use `data-icon="inline-start"` / `data-icon="inline-end"` for icons
   inside buttons instead of manual `size-4`/`mr-2`.
2. **Every size and color uses a named token.** Use `text-sm`, `gap-4`, `p-6`, `size-4`,
   `bg-primary`, `text-muted-foreground`, `border-border`, etc.
   Never use arbitrary values such as `text-[14px]`, `gap-[1.2rem]`, or `size-[18px]`.
3. **No raw Tailwind palette colors or inline color styles.** No `text-blue-500`,
   `bg-green-200`, `style={{ color: ... }}`. Use semantic tokens from
   `web/src/styles/globals.css` and utilities from `web/src/styles/design-system.css`.
4. **No `space-x-*` / `space-y-*`.** Use `flex gap-*` or `flex flex-col gap-*`.
5. **Use Base UI composition.** Prefer the `render` prop
   (`<Button render={<Link to={...} />}>`) over wrapping an interactive element in
   another interactive element. Never nest a button inside a link or vice versa.
6. **Class overrides are for layout only.** `className` may adjust spacing, width, and
   grid placement. Do not restyle primitive colors, borders, radii, or states ad hoc.
7. **No eager barrel imports.** Import the specific component module so charts, code
   highlighting, and carousel dependencies stay out of unrelated pages.

### Forms

- Build typed forms with React Hook Form + Zod, `FieldGroup`/`Field`, `FieldLabel`,
  `FieldDescription`, and `FieldError`.
- Bind labels with `htmlFor`; show server/ProblemDetails failures in `Alert` or root
  `FieldError`.
- Keep pending, disabled, validation, and keyboard states intact.

### Required user-visible states

Every changed workflow must cover loading (`Skeleton`/`Spinner`), empty (`Empty`), error
(`Alert`), pending mutation (disabled control + `Spinner`), success feedback (`Sonner`
`toast`), and mobile layout.

## Generated API output

- Do not edit `web/src/generated/**` directly.
- Change `contracts/openapi/api.v1.yaml`, update runtime contracts, and run the generation/verification workflow instead.

## Verification

```powershell
yarn --cwd web lint
yarn --cwd web format:check
yarn --cwd web build
```

The UI policy sensor fails on raw controls in feature code or a missing official
component. Run it before handoff whenever a `.tsx` file changed.
