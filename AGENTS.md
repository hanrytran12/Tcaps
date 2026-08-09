# Tcaps BE Agent Guide

This file contains repository-local guidance for the backend at D:\Clone\Tcaps\Tcaps. Workspace-level instructions still apply.

## Source of truth

- Canonical baseline inspected for this guide: branch master, commit 24ddaf8.
- [docs/](./docs/) is the source of truth for architecture, standards, deployment, roadmap, and project context.
- [README.md](./README.md) is the short onboarding entry point. Keep detailed explanations in docs/.
- Verify branch and deployment assumptions before describing them as policy; the current workflow mentions main and feature/db-seeder, while this repository is on master.

## Repository map

| Project | Responsibility |
|---|---|
| API | ASP.NET Core host, controllers, middleware, Swagger, auth, SignalR wiring |
| Application | CQRS/MediatR commands, queries, validators, DTOs, application services |
| Domain | Entities, value primitives, enums, domain events, repository contracts |
| Infrastructure | EF Core/SQL Server, migrations, repositories, integrations, background services |

Feature use cases belong under Application/Features/<Feature>. Keep transport concerns in API, business rules in Domain/Application, and external-system details in Infrastructure.

## Common commands

Run from the repository root:

~~~powershell
dotnet restore Tcaps.sln
dotnet build Tcaps.sln
dotnet run --project API/API.csproj --environment Development
~~~

Container workflow:

~~~powershell
docker compose up -d
docker compose ps
docker compose logs -f tcaps
docker compose down
~~~

The current Compose file pulls hanryhuy/tcaps:latest; source changes require a newly built image or an explicit local image override.

## Implementation rules

- Follow YAGNI, KISS, and DRY.
- Prefer the existing CQRS/MediatR, repository, DTO, validator, and DI patterns.
- Add a request/handler and validator for new use cases when the surrounding feature uses them.
- Keep controllers thin; do not move business workflows into route methods.
- Use Result/existing exception handling conventions instead of inventing a second response model.
- Preserve public API contracts and existing database migration history.
- Do not edit generated migration designer files manually; create migrations through EF tooling and review the generated diff.
- Keep new files focused. Split large classes instead of extending already oversized files.
- Match existing naming where modifying legacy code, but use clear, correctly spelled names for new code.

## API and security conventions

- API routes use api/[controller].
- Authentication is JWT bearer. Authorization uses roles and named policies configured in API/Program.cs.
- Swagger is enabled by the current host and supports the Bearer security scheme.
- SignalR notifications use /hubs/notificationHub; hub token handling is configured in the JWT events.
- OTP endpoints use the OtpPolicy rate limiter. Do not weaken limits without a security review.
- Never print, commit, copy, or document secret values. The tracked .env file is a known security risk; use variable names only and handle rotation/removal separately.

## Configuration

Important names include TCAPS_DB_CONNECTION, BLOB_STORAGE_SETTINGS, JWT_KEY, ISSUER, AUDIENCE, REDIS_PASSWORD, BREVO_API_KEY, BREVO_SENDER_NAME, and BREVO_SENDER_EMAIL. Read [docs/deployment-guide.md](./docs/deployment-guide.md) for the complete mapping.

Program.cs currently reads JWT values from environment variables. Do not assume the similarly named JwtSettings section in appsettings.json is the active source without verifying the code path.

## Verification before handoff

At minimum:

1. Run dotnet build Tcaps.sln after code changes.
2. Run available tests; do not claim coverage if no test project exists.
3. Check migration, configuration, and authorization changes for regressions.
4. Inspect git diff and ensure no .env, credential, token, or generated build output is included.
5. Update the relevant [docs/](./docs/) page when behavior, setup, architecture, or deployment changes.

## Documentation maintenance

Use Vietnamese prose by default while retaining English code identifiers and commands. Add a Last verified date to operational docs when checking ports, environment names, or deployment behavior. Record known mismatches and assumptions explicitly instead of silently normalizing them.
