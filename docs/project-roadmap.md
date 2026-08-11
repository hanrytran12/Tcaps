# Project Roadmap

> This is an engineering roadmap based on the current repository. It is not a committed release schedule.
>
> Last verified: 2026-08-12 against the current API refactor worktree.

## Current baseline

| Milestone | Status | Evidence |
|---|---|---|
| Layered .NET 8 backend | Complete baseline | Four projects in Tcaps.sln |
| API/auth/authorization | Phase 1 complete; Phase 2 hardening in progress | API/Program.cs, controllers, policies, api-contract-matrix.md |
| API controller composition | Phase 3 complete | Constructor-injected ISender across API controllers |
| API response contracts | Phase 4 complete | User, WorkshopInventory, Income, MaterialWorkshop, ReworkRequest, Inventory, and Batch DTO boundaries completed |
| Host/dependencies | Phase 5 complete | Service registration, host limits, middleware pipeline, and database initialization are composed through focused API extensions |
| SQL Server persistence | Active baseline | EF Core context, migrations, repositories |
| Startup migration/seeding | Active baseline | Program.cs, DbSeeder |
| Docker/Compose runtime | Active baseline | Dockerfile, compose.yml |
| Initial repository documentation | Complete in this change | README.md, AGENTS.md, docs/ |

## Recommended next steps

### P0 — Security hygiene

- Rotate any credentials exposed through the historical tracked .env and appsettings files.
- Remove secret material from tracking/history according to repository policy.
- Add safe ignore rules and a non-secret configuration example.
- Confirm CI secrets and local setup after rotation.

### P1 — Delivery reliability

- Confirm the canonical release/deployment branch (master, main, or feature/db-seeder).
- Align .github/workflows/cicd.yml, protected branches, and deployment documentation.
- Add readiness/health checks for SQL Server and Redis instead of relying only on startup timing/retries.
- Define rollback and database migration recovery procedures.

### P1 — Quality and contracts

- Add unit/integration tests for auth, authorization policies, state transitions, seeding, and critical repositories.
- Verify role/claim semantics for QCK and QCTransport with representative JWTs before release; mobile-compatible read permissions are now aligned in source.
- Establish API contract checks for the mobile/frontend consumers and add integration coverage for effective route authorization.
- Add structured logging and correlation IDs for workflow failures and background jobs.

### P2 — Maintainability

- Review inconsistent legacy naming and duplicate package/configuration patterns incrementally.
- Document domain state machines for material, assignment, QC, transfer, and rework workflows.
- Add automated Markdown/link and documentation freshness checks.

## Roadmap maintenance

Update this page when a milestone changes status, a major dependency changes, or a security/deployment decision is made. Do not add dates or release promises without an owner-approved plan.
