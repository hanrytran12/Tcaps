# Codebase Summary

> Last verified: 2026-08-09 against master at 24ddaf8.

## Solution shape

Tcaps.sln contains four .NET 8 projects:

~~~text
API/             Web host and HTTP/SignalR transport
Application/     Use cases and application orchestration
Domain/          Core business model and abstractions
Infrastructure/  Persistence and external integrations
~~~

Approximate baseline from the source scan (excluding .git, build output, and migration files): 581 tracked source/config files and about 20,084 lines. Counts are orientation data and will drift as the repository changes.

## API project

- Entry point: API/Program.cs.
- Controllers cover auth, users, products, materials, batches, assignments, production, inventory, workshops, QC, requests, transfers, notifications, evaluations, income, and rework.
- GlobalExceptionHandler provides centralized exception handling.
- Swagger, CORS, JWT bearer auth, authorization policies, rate limiting, static files, and SignalR are configured in the host.

## Application project

- Features/ is organized by business capability.
- Commands and queries are handled with MediatR.
- Validators are registered from the Application assembly.
- AutoMapper maps DTOs and domain/application models.
- Application services include staff and assignment-completion workflows.
- Domain event handlers trigger follow-up operations and notifications.

Feature areas currently include assignments, assignment transfer requests, auth, batches, component defects, evaluations, final transfer requests, incomes, inventories, material requests/materials/material supplies/material uses/material workshops, notifications, productions, products, rework requests, task transfer requests, users, workshops, and workshop inventory.

## Domain project

- Entities/ contains the production, material, inventory, user, workshop, request, transfer, notification, and evaluation model.
- Events/ contains domain events for workflow transitions and notifications.
- Interfaces/ contains repository and unit-of-work contracts used by upper layers.
- Enums/ and Primitives/ hold shared domain types.

## Infrastructure project

- Persistence/ contains AppDbContext, configurations, unit of work, and DbSeeder.
- Migrations/ contains the EF Core migration history and snapshot.
- Repositories/ contains entity-specific and generic data access.
- Services/ integrates JWT, password hashing, Blob Storage, email, OTP, notifications, and assignment automation.
- BackgroundService/DeadlineCheckerService.cs performs hosted deadline checks.
- Hubs/ contains SignalR notification support and user ID mapping.
- Behaviors/ contains transaction behavior used in the request pipeline.

## Runtime dependencies

- SQL Server is the primary database.
- Redis is configured as a distributed cache.
- Azure Blob Storage is required during infrastructure registration.
- Email/Brevo settings support email and OTP flows.
- Docker Compose defines API, SQL Server, and Redis services.

## Important entry points

- Local host/configuration: API/Program.cs
- Application DI: Application/DependencyInjection.cs
- Infrastructure DI: Infrastructure/DependencyInjection.cs
- API project: API/API.csproj
- Container build: Dockerfile
- Container topology: compose.yml
- CI/CD: .github/workflows/cicd.yml
