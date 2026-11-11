---
name: component-first
description: "Component-first UI workflow for Solution web/: choose and compose the installed shadcn base-nova primitives instead of hand-writing semantic HTML, audit a diff for raw controls, and fix violations. Use when adding or reviewing any JSX in web/**, when a page needs buttons, inputs, tables, menus, dialogs, alerts, or navigation, or when asked to make the UI component-driven."
---

# Component-First Skill

Use this skill before writing or reviewing JSX under `web/`. It turns the rules in
`web/AGENTS.md` into a selection and audit procedure. `$frontend` owns the wider workflow;
this skill owns component choice and raw-HTML compliance.

## Non-Negotiable

The repo ships the complete official shadcn `base-nova` set in
`web/src/components/ui`. Feature code composes those primitives. A raw interactive
element is a defect, not a shortcut.

Allowed semantic HTML outside `web/src/components/ui/**` is limited to:

- landmarks/layout: `div`, `span`, `main`, `header`, `footer`, `section`, `article`,
  `aside`, `form`;
- prose: `h1`-`h6`, `p`, `ul`, `ol`, `li`, `blockquote`, `pre`, `code`, `strong`, `em`;
- navigation links: `a` / React Router `Link`;
- media: `img`.

Everything else has a primitive. See `web/AGENTS.md` for the banned list and
`web/src/components/ui/README.md` for the installed groups.

## Selection Pass

1. **Inventory the UI affordances** in the slice: actions, inputs, choices, overlays,
   navigation, data display, feedback, surfaces, media.
2. **Map each affordance to a primitive** using the table in `web/AGENTS.md`.
3. **Read the primitive module** (`web/src/components/ui/<name>.tsx`) for exported parts,
   variants, sizes, and the Base UI `render` prop.
4. **Compose the full group.** Examples:
   - page surface: `Card` + `CardHeader` + `CardTitle` + `CardDescription` +
     `CardContent` + `CardFooter`;
   - form row: `Field` + `FieldLabel` + `Input` + `FieldDescription` + `FieldError`;
   - menu: `DropdownMenu` + `DropdownMenuTrigger` + `DropdownMenuContent` +
     `DropdownMenuItem` + `DropdownMenuSeparator`;
   - navigation: `NavigationMenu` + `NavigationMenuList` + `NavigationMenuItem` +
     `NavigationMenuLink`;
   - overlay: `Dialog`/`AlertDialog` + trigger/content/header/title/footer parts;
   - table: `Table` + `TableHeader` + `TableBody` + `TableRow` + `TableHead` +
     `TableCell`.
5. **Use variants, not new styles.** `variant`, `size`, `data-icon="inline-start|end"`,
   and semantic tokens. No arbitrary sizes, raw palette colors, inline color styles, or
   `space-x-*`/`space-y-*`.
6. **If no primitive fits**, stop and choose explicitly:
   - compose existing primitives;
   - add a primitive through `$shadcn` + `web/src/components/ui/AGENTS.md`;
   - request a documented semantic-HTML exception in the handoff.
   Never fall back to a raw control.

## Base UI Composition

The primitives use Base UI. Use the `render` prop for polymorphism:

```tsx
<Button render={<Link to="/account" />}>Account</Button>
```

Do not nest interactive elements. Do not mix Radix `asChild` patterns into a Base UI
primitive. Do not wrap a `Button` in a `Link` (or vice versa).

## Audit Procedure

Run the sensor first:

```powershell
```

Then inspect the diff for what a regex cannot judge:

- Is a component group split across ad-hoc `<div>`s that its own parts should own?
- Does `className` override primitive colors, borders, radii, or states instead of layout?
- Are variants reimplemented locally (a second button style, a custom modal, a custom table)?
- Are labels, descriptions, and errors wired through `Field` rather than bare text?
- Are loading, empty, error, pending, and success states present?
- Is any raw element justified by a documented exception, not convenience?

Scoped audit for a file list:

```powershell
```

## Fix Recipes

| Violation | Fix |
|---|---|
| `<button className="...">` | `<Button variant="...">` |
| `<input>` | `<Input>` inside `Field` + `FieldLabel` |
| `<select><option>` | `<Select>` or `<NativeSelect>` |
| `<table>` | `Table` family |
| `<dialog>` / custom modal div | `Dialog` or `AlertDialog` |
| `<nav><a>` | `NavigationMenu` (or `Breadcrumb` for location) |
| `<hr>` | `Separator` |
| `<details><summary>` | `Accordion` or `Collapsible` |
| `<progress>` | `Progress` |
| custom toast/banner | `Sonner` `toast()` / `Alert` |
| empty list text | `Empty` family |
| hand-rolled spinner | `Spinner` or `Skeleton` |
| raw icon button | `Button size="icon"` with `aria-label` |

## Verification

```powershell
yarn --cwd web lint
yarn --cwd web build
```

`yarn lint` enforces the banned-element list. The sensor additionally verifies that the
official component inventory is complete. Use browser verification for focus, keyboard,
overlay, and responsive behavior.
