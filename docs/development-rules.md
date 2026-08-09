# Development Rules

> Last verified: 2026-08-09 against master at 24ddaf8.

This document defines the minimum rules for changing the Tcaps backend. AGENTS.md is the quick operational summary; this page is the detailed reference.

## Principles

- Apply YAGNI, KISS, and DRY.
- Prefer small, focused changes that match existing architecture.
- Preserve API, database, and authorization contracts unless the change explicitly includes migration and consumer coordination.
- Document behavior changes in the relevant docs/ page.

## Project boundaries

- API: transport, routing, middleware, host configuration, and external HTTP-facing concerns.
- Application: use cases, CQRS requests, validation, DTO mapping, and application services.
- Domain: business entities, domain events, enums, primitives, and abstractions; no infrastructure implementation.
- Infrastructure: persistence, repositories, integrations, hosted services, and external-system adapters.

Dependency direction should point inward toward Domain abstractions. New business behavior should not be implemented directly in controllers or repository classes.

## Code conventions

- Follow existing C# nullable and implicit-using settings.
- Use PascalCase for public types/members and camelCase for local variables/parameters.
- Use feature folders and descriptive names for new requests, handlers, validators, DTOs, and event handlers.
- Keep controllers thin and handlers explicit about their inputs and outputs.
- Reuse Result, PagedResult, existing exceptions, and GlobalExceptionHandler patterns.
- Register new services in the appropriate dependency-injection extension.
- Keep new classes focused and split large files when responsibilities diverge.
- Preserve legacy spellings when modifying an existing public type; do not create new misspellings.

## Data and migrations

- Use AppDbContext and existing repository/unit-of-work abstractions.
- Review entity configuration and migration impact together.
- Generate migrations with EF tooling; do not hand-edit generated designer/snapshot files.
- Treat startup migration and DbSeeder as deployment behavior. Test against a disposable database before production use.

## API and security

- Use api/[controller] and existing HTTP verb conventions.
- Apply the narrowest existing role/policy authorization requirement.
- Never log JWTs, passwords, OTPs, connection strings, or provider keys.
- Keep secret values out of source, docs, examples, screenshots, and commits.
- Review CORS, file upload limits, static-file behavior, rate limiting, and SignalR token handling when touching host configuration.

## Verification

~~~powershell
dotnet restore Tcaps.sln
dotnet build Tcaps.sln
git diff --check
~~~

Run available tests. The current baseline has no discovered test project, so do not claim test coverage. For database or deployment changes, validate Compose configuration and startup migration behavior with safe non-production credentials.

## Documentation maintenance

- Use docs/ as the source of truth and link from README.md.
- Use Vietnamese prose by default; retain English identifiers and commands.
- Add a Last verified date to operational documents.
- Record known mismatches such as branch/deployment drift instead of hiding them.
