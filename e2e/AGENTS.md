# Solution end-to-end testing instructions

## Scope

Applies to `e2e/**`.

## Purpose

Use Playwright for critical browser journeys that cannot be covered more cheaply by API or unit
tests. The starter e2e suite focuses on register, login, logout, session bootstrap, and protected
route guards.

## Structure

- Keep scenarios under the existing feature-oriented test folders.
- Put repeated UI interactions and selectors in page objects.
- Put fictional seed/build data in fixtures and reusable behavior in helpers.

## Rules

- Prefer role, label, and test-id selectors that express user intent.
- Avoid fixed sleeps. Wait for observable UI, network, or state transitions.
- Keep tests independent, deterministic, and safe to run in any order.
- Do not commit `test.only`, skipped critical tests, screenshots/videos containing sensitive data, or `.env` files.
- Use only fictional names, emails, passwords, and users.

## Verification

```powershell
yarn --cwd e2e test
```
