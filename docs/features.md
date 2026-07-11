---
doc_schema: solution-doc-v1
doc_kind: source_spec
doc_id: solution.features
title: Solution Feature Skeleton
status: template
version: "1.0"
last_updated: 2026-07-11
owner: builder
language: en
authority: observable_product_behavior_acceptance_and_delivery_status
source_documents:
  - docs/product.md
  - docs/technical.md
---

# Solution Feature Skeleton (`features.md`)

This file is a template for observable product behavior. Keep it current when adding real product
features to the boilerplate.

## Specification Contract

Feature records define user-visible behavior and Acceptance criteria. Product intent belongs in
[`product.md`](product.md). Architecture and verification strategy belong in
[`technical.md`](technical.md).

## Epic Index

| Epic | Title | Status |
|---|---|---|
| EP-1 | Users and Authentication | implemented |
| EP-2 | Authorization Skeleton | implemented |
| EP-TEMPLATE | Future Product Feature | template |

## Delivery Snapshot

| Scope | Delivery status | Evidence |
|---|---|---|
| User registration | implemented | API, frontend, integration tests, e2e tests. |
| Sign in, sign out, session bootstrap | implemented | API, frontend, integration tests, e2e tests. |
| Role-protected admin endpoint | implemented | API endpoint and authorization policy. |
| Product-specific business behavior | not included | Removed from boilerplate. |

## Feature Records

### F-1.1 - Register a user

Acceptance criteria:

- Given a unique email and valid password, when a visitor registers, then a user is created and a
  session starts.
- Given an email already in use, when a visitor registers, then registration is rejected.

### F-1.2 - Sign in and sign out

Acceptance criteria:

- Given valid credentials, when a user signs in, then the browser receives an authenticated session.
- Given an active session, when a user signs out, then protected routes require sign in again.

### F-2.1 - Enforce backend authorization

Acceptance criteria:

- Given an unauthenticated caller, when a protected endpoint is requested, then the API returns 401.
- Given a non-admin user, when an admin-only endpoint is requested, then the API returns 403.
- Given an admin user, when an admin-only endpoint is requested, then the API returns 200.

## Maintenance Rule

This skeleton intentionally avoids product-specific roadmap content. Add new feature records here
only when the boilerplate is adapted into a product.
