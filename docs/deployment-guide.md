# Deployment Guide

> Last verified: 2026-08-15 against `refactor/api-contract-sync`.

## Deployment model

The repository contains:

- Dockerfile: multi-stage .NET 8 build/publish for API.
- compose.yml: API image, SQL Server, and Redis services with persistent volumes.
- .github/workflows/cicd.yml: Docker build/push for pushes to main and feature/db-seeder; deploy and Telegram notification steps are conditional on feature/db-seeder.

The current checked-out source branch is master, so the workflow branch rules must be confirmed before being treated as the production release policy.

## Configuration contract

Provide values through a secure local environment, CI secret store, or deployment secret manager. This document intentionally lists names only.

| Name | Used for |
|---|---|
| TCAPS_DB_CONNECTION | Primary SQL Server connection string; preferred in container mode |
| ConnectionStrings__DefaultConnection | Fallback/configuration-based SQL connection string |
| TCAPS_SEED_PASSWORD | Password used to hash startup seed users |
| BLOB_STORAGE_SETTINGS | Azure Blob Storage connection string |
| JWT_KEY | JWT signing key |
| ISSUER | JWT issuer |
| AUDIENCE | JWT audience |
| ConnectionStrings__RedisConnection | Redis connection string used by the API |
| REDIS_PASSWORD | Redis container password and Compose interpolation input |
| BREVO_API_KEY | Email provider API key passed into BrevoSettings__ApiKey |
| BREVO_SENDER_NAME | Email sender name |
| BREVO_SENDER_EMAIL | Email sender address |
| SA_PASSWORD | SQL Server container password |
| ACCEPT_EULA | SQL Server container license acceptance |
| ASPNETCORE_ENVIRONMENT | ASP.NET environment selection |

The host and JwtTokenGenerator read JWT environment variables first. Keep real JWT values out of appsettings files; the configuration paths are only a controlled fallback.

## Local .NET workflow

~~~powershell
dotnet restore Tcaps.sln
dotnet build Tcaps.sln
dotnet run --project API/API.csproj --environment Development
~~~

The host searches the current and parent directories for .env. Do not commit or copy real credentials. Historical revisions contained a tracked .env; keep local copies ignored and rotate every exposed value.

## Compose workflow

~~~powershell
docker compose up -d
docker compose ps
docker compose logs -f tcaps
docker compose down
~~~

Services:

- tcaps: API image hanryhuy/tcaps:latest, ports 5000 and 5001.
- tcapdb: SQL Server 2022, port 1433, volume tcapdbdata.
- tcaps_redis: Redis Alpine, port 6379, volume redisdata.

The Compose file pulls a prebuilt API image. For local source changes, build/tag the image explicitly or use a development override; docker compose up alone will not build the working tree.

## Database startup behavior

When the API starts, it calls EF Core MigrateAsync and DbSeeder.SeedAllAsync. It retries database startup up to five times with a five-second delay. depends_on orders container startup but does not itself prove SQL Server readiness.

Validate migrations and seed data against a disposable database before production deployment. Do not run destructive migration or volume cleanup commands against production without an approved backup and rollback plan.

## Runtime endpoints

- Swagger UI: http://<host>:5000/swagger
- HTTP/1.1 API listener: <host>:5000
- HTTP/2 listener: <host>:5001
- SignalR notification hub: /hubs/notificationHub

The current host enables Swagger without an environment guard. Confirm exposure policy before publishing a production endpoint.

## CI/CD behavior

The workflow:

1. Checks out the source.
2. Creates .env from the PROD_ENV GitHub secret.
3. Logs into Docker Hub.
4. Builds and pushes hanryhuy/tcaps:latest.
5. On feature/db-seeder, pulls/recreates containers on the VPS and sends a Telegram notification.

Before relying on this workflow, verify branch protection, GitHub environment ENV, Docker credentials, VPS Compose configuration, and rollback behavior. Never paste CI secret contents into repository docs or issue comments.

## API refactor release gate

Before deploying the API refactor:

1. Rotate all credentials that were present in historical tracked environment files.
2. Run `dotnet restore Tcaps.sln`, `dotnet build Tcaps.sln`, and `dotnet test Tcaps.sln --no-restore`; the current focused suite has 44 passing tests.
3. Verify representative JWTs for Admin, Lead, QC, QCK, QCTransport, GuardQC, and Staff against the contract matrix.
4. Smoke-test login, password recovery, batch list/detail, material request, production report, notifications, user profile, and workshop inventory flows.
5. Keep the previous image/tag and database backup available before rollout; rollback application image and configuration together if authorization or contract smoke tests fail.

## Troubleshooting checklist

- Check docker compose ps and docker compose logs -f tcaps tcapdb tcaps_redis.
- Confirm the API can resolve tcapdb and tcaps_redis from the Compose network.
- Confirm TCAPS_DB_CONNECTION, TCAPS_SEED_PASSWORD, Blob Storage, JWT, Redis, and email settings are present.
- If startup retries are exhausted, inspect SQL Server readiness, credentials, and migration compatibility.
- If clients cannot authenticate, verify issuer, audience, signing key, clock skew, and role/claim values.
- If clients cannot connect to SignalR, verify the hub path and token transport behavior.
