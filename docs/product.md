---
doc_schema: solution-doc-v1
doc_kind: source_spec
doc_id: solution.product
title: Solution Product Skeleton
status: template
version: "1.0"
last_updated: 2026-07-11
owner: builder
language: en
authority: product_intent_scope_outcomes_and_decisions
source_documents:
  - docs/features.md
  - docs/technical.md
---

# Solution Product Skeleton (`product.md`)

This file is a boilerplate placeholder for product intent. Replace the template text when this
starter becomes a real product.

## Specification Contract

`product.md` defines the allowed outcome and scope. Observable behavior belongs in
[`features.md`](features.md). Implementation mechanisms belong in [`technical.md`](technical.md).

## Product Intent

Solution is an application starter focused on users, authentication, authorization, a React web
shell, a .NET API, PostgreSQL persistence, Redis-backed sessions, object storage for user
avatars, and Aspire local orchestration.

## Personas

| ID | Persona | Core need |
|---|---|---|
| PER-1 | Application user | Register, sign in, keep a secure session, and view protected app content. |
| PER-2 | Administrator | Access role-protected backend operations. |

## Goals

| ID | Goal | Intent |
|---|---|---|
| G-1 | Ship a reusable starter | Keep only cross-product account and app-shell capabilities. |
| G-2 | Stay maintainable | Preserve Clean Architecture boundaries and focused verification. |
| G-3 | Keep security central | Enforce protected operations on the backend. |

## Non-Goals

- Product-specific business domains.
- Product-specific feature roadmaps.
- Product-specific workflows.

## Maintenance Rule

This is one of exactly three authoritative Solution specifications. Product summaries, MOCs,
duplicate decision logs, and separate product-intent documents MUST NOT become competing sources of
truth.
