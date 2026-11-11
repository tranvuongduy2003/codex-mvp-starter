# Component Workflow

Use this reference when composing pages or deciding which primitive owns an affordance.
Rules live in `web/AGENTS.md` and `web/src/components/ui/AGENTS.md`; this file is the
procedural path through them.

## Scout

Read:

- `web/src/components/ui/README.md` for installed groups and dependency notes.
- The exact primitive module you intend to use; confirm exports, variants, and sizes.
- Nearby feature components for composition style.
- `web/components.json` when shadcn/Base UI conventions matter.

Useful commands:

```powershell
rg --files web/src/components/ui
rg "export \{|export function" web/src/components/ui/<component>.tsx
rg "<button|<input|<select|<textarea|<table|<dialog" web/src --glob '!components/ui/**'
```

## Select

| Need | Primitive |
|---|---|
| Action | `Button`, `ButtonGroup` |
| Text entry | `Input`, `Textarea` |
| Form field anatomy | `Field`, `FieldLabel`, `FieldDescription`, `FieldError`, `FieldGroup` |
| Select / combobox | `Select`, `NativeSelect`, `Combobox` |
| Boolean / choice | `Checkbox`, `RadioGroup`, `Switch`, `Toggle`, `ToggleGroup`, `Slider` |
| Overlay | `Dialog`, `AlertDialog`, `Sheet`, `Drawer`, `Popover`, `HoverCard`, `Tooltip` |
| Navigation | `NavigationMenu`, `Breadcrumb`, `Sidebar`, `Pagination` |
| Menu | `DropdownMenu`, `ContextMenu`, `Menubar`, `Command` |
| Surface | `Card` family, `Separator`, `ScrollArea`, `Resizable`, `AspectRatio` |
| Data | `Table` family, `Badge`, `Avatar`, `Progress`, `Chart` |
| Feedback | `Alert`, `Empty`, `Spinner`, `Skeleton`, `Sonner` (`toast`) |
| Disclosure | `Accordion`, `Collapsible`, `Tabs` |
| List / metadata | `Item` family, `Kbd`, `Marker` |
| Media / carousel | `Carousel`, `AspectRatio` |
| Code / stream | `CodeBlock`, `Message`, `MessageScroller`, `Bubble`, `Trace`, `Attachment` |

If nothing fits, compose existing primitives first. Add a primitive only when the need is
repeated and the accessibility behavior is genuinely new; use `$shadcn` and follow
`web/src/components/ui/AGENTS.md`.

## Apply

- Import direct modules (`@/components/ui/button`), not a barrel.
- Compose the full group, not an isolated part.
- Prefer `variant`/`size` and `data-icon` over local color/size classes.
- Use Base UI `render` for polymorphism; never nest interactive elements.
- Keep feature-specific components inside the feature; primitives stay presentational.
- Preserve accessible names, labels, disabled/pending states, and keyboard behavior.
- Cover loading, empty, error, pending, success, and mobile states.

## Review the diff

Reject the change if it contains any of these outside `web/src/components/ui/**`:

- raw `button`, `input`, `select`, `option`, `optgroup`, `textarea`, `label`,
  `fieldset`, `legend`, `progress`, `meter`, `output`, `table`, `thead`, `tbody`,
  `tfoot`, `tr`, `th`, `td`, `caption`, `dialog`, `details`, `summary`, `hr`, `nav`;
- arbitrary sizing (`text-[14px]`, `size-[18px]`) or raw palette colors (`text-blue-500`);
- inline color/background styles;
- `space-x-*` / `space-y-*`;
- a second icon library or duplicated primitive.

## Verify

```powershell
yarn --cwd web lint
yarn --cwd web build
```

Use browser verification for layout, focus, menus/dialogs, responsive behavior, or any
interaction TypeScript cannot prove.
