# UI components

This directory contains the complete installable official shadcn `base-nova` UI set
available when installed with shadcn CLI 4.23.0, plus the existing Sonner adapter.
Feature code must compose these primitives instead of hand-writing interactive HTML;
see `web/AGENTS.md` for the component-first contract and the banned raw element list.
Import individual components directly; there is no eager barrel that adds charts,
code highlighting, or carousel dependencies to every page.

## Design system

- `src/styles/globals.css` owns the light-only Solution palette, Plus Jakarta Sans,
  radii, shadows, chart colors, overlay color, and sidebar tokens.
- Sidebar colors alias the shared surface, primary, accent, border, and focus tokens.
- Components use Base UI's `render` composition API and Lucide icons.
- Reuse `@/lib/utils` for `cn` and `button-variants.ts` for button styling.
- `TooltipProvider` is mounted in the app providers. Application notifications use
  the existing `sonner.tsx` adapter and `toast()` from `sonner`.
- `toast.tsx` also exposes the official Base UI toast primitive for explicit
  composition; it is not mounted alongside Sonner.
- Use `Field`, `FieldGroup`, and React Hook Form for forms. The Base UI registry's
  `form` entry is a recipe, not a separate installable `form.tsx`.
- Date pickers and data tables are composition recipes using Calendar/Popover and
  Table, respectively. Typography uses semantic HTML and the shared font tokens.
- The existing collapsible API exports `CollapsibleRoot` and `CollapsiblePanel`.
  Trace adapts those names without changing existing callers.

## Installed groups

| Group                | Components                                                                                                                                                 |
| -------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Actions              | Button, Button Group, Toggle, Toggle Group                                                                                                                 |
| Inputs               | Calendar, Checkbox, Combobox, Field, Input, Input Group, Input OTP, Label, Native Select, Radio Group, Select, Slider, Switch, Textarea                    |
| Navigation           | Breadcrumb, Command, Context Menu, Direction, Dropdown Menu, Menubar, Navigation Menu, Pagination, Sidebar, Tabs                                           |
| Surfaces             | Accordion, Alert Dialog, Aspect Ratio, Card, Carousel, Collapsible, Dialog, Drawer, Hover Card, Popover, Resizable, Scroll Area, Separator, Sheet, Tooltip |
| Display and feedback | Alert, Avatar, Badge, Chart, Empty, Item, Kbd, Progress, Skeleton, Sonner, Spinner, Table, Toast                                                           |
| Content primitives   | Attachment, Bubble, Code Block, Marker, Message, Message Scroller, Questionnaire, Trace                                                                    |

Content primitives are installed as reusable UI only; their presence does not add
chat, AI, commerce, or other product workflows.

## Dependency license review

The full component set adds the following production dependencies.
Their package metadata declares MIT licenses; React peer ranges were checked
against the app's React 19 setup. Keep their license files when redistributing.

| Dependency               | Purpose                                     |
| ------------------------ | ------------------------------------------- |
| `@shadcn/react`          | Message Scroller and Questionnaire behavior |
| `cmdk`                   | Command filtering and keyboard interaction  |
| `date-fns`               | Calendar date utilities                     |
| `embla-carousel-react`   | Carousel behavior                           |
| `input-otp`              | OTP input behavior                          |
| `react-day-picker`       | Calendar behavior                           |
| `react-is`               | Required Recharts peer dependency           |
| `react-resizable-panels` | Resizable panel behavior                    |
| `recharts`               | Chart rendering; registry version 3.8.0     |
| `shiki`                  | Lazy language loading for code highlighting |

The upstream `cn` dependency was replaced with the existing local helper rather
than retaining a second class-merging implementation. Existing locked package
versions are preserved. Review new dependencies and licenses again when updating
registry components; do not overwrite local adaptations blindly.

The added transitive packages declare MIT, ISC, BSD-3-Clause, or the combined
`MIT AND ISC AND BSD-3-Clause` license (Victory's bundled D3 code). These are
permissive licenses requiring preservation of copyright and license notices;
BSD-3-Clause also prohibits endorsement using contributors' names.
