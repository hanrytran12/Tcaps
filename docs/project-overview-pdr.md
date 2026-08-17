# Project Overview and PDR

> Status: baseline inferred from the current source. Product requirements not represented in code require product-owner confirmation.
>
> Last verified: 2026-08-15 against `refactor/api-contract-sync`; product assumptions remain inferred from source.

## Product context

TCAPS is a production operations backend for coordinating materials, workshops, batches, assignments, quality control, transfers, inventory, staff, and notifications. The domain model indicates a workflow that moves work and materials through production and QC stages while recording approvals, quantities, defects, and outcomes.

This description is an engineering baseline, not a replacement for a product requirements document.

## Observed capabilities

| Area | Evidence in source |
|---|---|
| Identity and access | AuthController, UserController, JWT policies, role checks |
| Production | batches, assignments, production reports, deadline checking |
| Materials | materials, requests, uses, supplies, workshop material flows |
| Inventory | inventory and workshop inventory repositories/controllers |
| Quality | evaluations, component defects, QC and rework requests |
| Transfers | assignment, task, and final transfer request features |
| Communication | notifications, SignalR hub, email/OTP services |
| Reporting | staff dashboards, group progress, income and production queries |

## Observed actors and access model

The host configures policies/roles for Admin, Lead, QC, QCK, GuardQC, and QCTransport, plus staff-oriented flows. Actual access is action-specific; clients must use Swagger/source authorization attributes as the contract.

## Product goals suggested by the code

1. Provide one API for production and material state transitions.
2. Enforce role-aware approvals and QC checkpoints.
3. Keep inventory and material quantities traceable across workshops and assignments.
4. Notify stakeholders when workflow events, shortages, deadlines, or approvals occur.
5. Support operational dashboards and income/production reporting.

## Requirements boundary

### In scope for this backend baseline

- HTTP API, JWT authentication, role/policy authorization, and Swagger.
- EF Core persistence with SQL Server migrations and startup seeding.
- Redis caching, Blob Storage, email/OTP, SignalR notifications, and background deadline checks.
- Domain events and MediatR-driven use cases.

### Not confirmed by source alone

- Exact business definitions of each role and approval SLA.
- Production data retention, audit/compliance requirements, and reporting correctness thresholds.
- Deployment environments, release branch policy, uptime/SLO targets, and backup/restore policy.
- Whether JwtSettings in appsettings is still supported; the host currently reads JWT values from environment variables.

## Acceptance baseline for future changes

- The workflow remains role-safe and transactionally consistent.
- API and database changes are backward-compatible or documented with migration/consumer steps.
- Notifications and background work fail visibly and do not silently corrupt workflow state.
- Operational documentation is updated when behavior or deployment changes.
