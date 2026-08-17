# Tcaps Backend

Backend API for the TCAPS production, material, inventory, quality-control, transfer, notification, and user workflows.

## Capabilities

- JWT authentication, registration, password reset, OTP verification, and role/policy authorization.
- Production batches, assignments, material requests/uses/supplies, inventory, workshops, and transfer requests.
- QC, rework, evaluation, income, user/staff, and dashboard workflows.
- SignalR notifications, email/OTP integration, Azure Blob file storage, Redis caching, and deadline background checks.
- EF Core migrations and startup database seeding.

## Technology stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 with SQL Server
- MediatR, FluentValidation, and AutoMapper
- JWT Bearer authentication and SignalR
- Redis, Azure Blob Storage, and Brevo-compatible email integration
- Docker, Docker Compose, and GitHub Actions

## Repository structure

~~~text
API/             ASP.NET host, controllers, middleware, Swagger, SignalR setup
Application/     CQRS/MediatR features, DTOs, validators, application services
Domain/          Entities, enums, domain events, repository contracts
Infrastructure/  EF Core, migrations, repositories, integrations, hosted services
docs/            Project, architecture, standards, deployment, and roadmap docs
~~~

See [AGENTS.md](./AGENTS.md) for repository rules and [docs/codebase-summary.md](./docs/codebase-summary.md) for a guided map.

## Prerequisites

- .NET 8 SDK
- Docker Engine/Desktop and Docker Compose v2 for the container workflow
- SQL Server and Redis, either locally or through Compose
- Valid configuration for JWT, database, Blob Storage, email, and Redis when exercising those integrations

## Configuration

The host searches the current directory and parent directories for .env. Do not commit or share secret values.

| Purpose | Configuration names |
|---|---|
| SQL Server | TCAPS_DB_CONNECTION, ConnectionStrings__DefaultConnection |
| Startup seed account password | TCAPS_SEED_PASSWORD |
| Redis | ConnectionStrings__RedisConnection, REDIS_PASSWORD (Compose expansion) |
| JWT | JWT_KEY, ISSUER, AUDIENCE |
| Azure Blob | BLOB_STORAGE_SETTINGS or BlobStorageSettings__ConnectionString |
| Email | BREVO_API_KEY, BREVO_SENDER_NAME, BREVO_SENDER_EMAIL |
| SQL Server container | SA_PASSWORD, ACCEPT_EULA |

## Run locally

From the repository root:

~~~powershell
dotnet restore Tcaps.sln
dotnet build Tcaps.sln
dotnet run --project API/API.csproj --environment Development
~~~

The host is configured for:

- Swagger UI: http://localhost:5000/swagger
- HTTP/1.1 API listener: port 5000
- HTTP/2 listener: port 5001
- SignalR notification hub: /hubs/notificationHub

On startup the API applies EF Core migrations and runs DbSeeder with up to five retries when the database is not ready. It requires a valid database connection and Blob Storage configuration during service registration.

## Run with Docker Compose

~~~powershell
docker compose up -d
docker compose ps
docker compose logs -f tcaps
docker compose down
~~~

Compose defines tcaps, tcapdb (SQL Server), and tcaps_redis (Redis), with persistent volumes for database and Redis data.

## API usage

Use Swagger to inspect the current controller contract and authorize with a Bearer JWT. Controllers are grouped by resource under api/[controller]; authorization requirements are defined in source and can differ by action.

For architecture and request flow, read [docs/system-architecture.md](./docs/system-architecture.md). For the route, binding, response, and authorization inventory, read [docs/api-contract-matrix.md](./docs/api-contract-matrix.md). For response mapping details, read [docs/api-response-contract.md](./docs/api-response-contract.md). For mobile usage evidence, read [docs/api-endpoint-usage-ledger.md](./docs/api-endpoint-usage-ledger.md). For environment, Docker, and CI/CD details, read [docs/deployment-guide.md](./docs/deployment-guide.md).

## Verification

~~~powershell
dotnet build Tcaps.sln
dotnet test Tcaps.sln --no-restore
git status --short
git -c core.autocrlf=false diff --check
~~~

The current BE test suite contains 44 passing tests.

## Documentation index

- [AGENTS.md](./AGENTS.md) — repository guidance for agents and developers
- [docs/project-overview-pdr.md](./docs/project-overview-pdr.md) — product context and requirement boundaries
- [docs/codebase-summary.md](./docs/codebase-summary.md) — source map and current baseline
- [docs/code-standards.md](./docs/code-standards.md) — coding and architecture conventions
- [docs/development-rules.md](./docs/development-rules.md) — contribution, security, and verification rules
- [docs/system-architecture.md](./docs/system-architecture.md) — runtime and request architecture
- [docs/api-contract-matrix.md](./docs/api-contract-matrix.md) — route, binding, response, and authorization matrix
- [docs/api-response-contract.md](./docs/api-response-contract.md) — Result-to-HTTP and client response contract
- [docs/api-endpoint-usage-ledger.md](./docs/api-endpoint-usage-ledger.md) — mobile static usage audit and deprecation guardrails
- [docs/project-roadmap.md](./docs/project-roadmap.md) — evidence-based next steps
- [docs/project-changelog.md](./docs/project-changelog.md) — implementation and verification history
- [docs/deployment-guide.md](./docs/deployment-guide.md) — local/container/CI deployment notes
