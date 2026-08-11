# System Architecture

> Last verified: 2026-08-11 against the current API refactor worktree.

## High-level topology

~~~mermaid
flowchart LR
    Client["Web/mobile clients"] --> Api["ASP.NET Core API\nAPI/Program.cs"]
    Api --> Controllers["Controllers\napi/[controller]"]
    Api --> Hub["SignalR hub\n/hubs/notificationHub"]
    Controllers --> Pipeline["MediatR pipeline\nvalidation + transaction"]
    Pipeline --> App["Application handlers\ncommands/queries/events"]
    App --> Domain["Domain entities/events"]
    App --> Persistence["Repositories + AppDbContext"]
    Persistence --> Sql[("SQL Server")]
    Api --> Redis[("Redis cache")]
    App --> Blob["Azure Blob Storage"]
    App --> Mail["Email/OTP provider"]
    Background["DeadlineCheckerService"] --> Persistence
    App --> Hub
~~~

## Request flow

1. A client calls a controller route under api/[controller].
2. ASP.NET authentication validates the JWT; authorization policies are registered through `API/Extensions/AuthorizationExtensions.cs` and check roles or named claims.
3. The controller dispatches a MediatR request through constructor-injected `ISender`.
4. Application validation and transaction behaviors run around the handler.
5. The handler uses Domain objects and Infrastructure abstractions for persistence/integrations.
6. EF Core persists changes to SQL Server; API-safe results are mapped to DTOs while legacy entity responses are being migrated feature by feature.
7. Domain events can invoke notification, inventory, production, or follow-up handlers.
8. The API returns a result/error response through the existing middleware conventions.

## Host responsibilities

API/Program.cs configures:

- environment loading from .env in the current or parent directory;
- controllers and Swagger with Bearer authentication;
- Application and Infrastructure dependency injection;
- CORS for the configured frontend origins;
- JWT validation using JWT_KEY, ISSUER, and AUDIENCE environment variables;
- role/policy authorization with an authenticated-user fallback policy and explicit anonymous auth/recovery actions;
- SignalR and hub user ID mapping;
- Redis distributed caching;
- OTP rate limiting;
- forwarded headers, static files, request-size limits, and exception handling;
- Kestrel listeners on ports 5000 (HTTP/1.1) and 5001 (HTTP/2);
- startup EF migration and DbSeeder execution with five retry attempts.

## Application patterns

Application features use MediatR commands, queries, validators, and notification/event handlers. This keeps transport logic in API and lets workflow side effects be composed from domain/application events. The pattern is not perfectly uniform across all legacy features; follow the nearest existing implementation when extending one.

## Persistence and consistency

AppDbContext implements the application database abstraction and unit-of-work contract. Infrastructure registers SQL Server with EF retry-on-failure and a transaction pipeline behavior. Multi-entity workflow changes should preserve the existing transaction boundary and domain-event behavior.

## External integrations

- Redis is used through the distributed cache registration.
- Azure Blob Storage is registered as a singleton client and used by file-storage services.
- Email/OTP services use configured provider settings and an HTTP client.
- SignalR publishes realtime notifications to connected clients.
- DeadlineCheckerService runs as a hosted background service.

## Security boundaries

- JWT tokens are validated before protected controller actions.
- The fallback authorization policy requires authentication unless an action explicitly opts into anonymous access.
- Named policies restrict operations such as admin, lead, QC, QC transport, and dashboard access.
- User lookup responses use DTO projection and do not expose `PasswordHash`.
- OTP requests are rate-limited by remote IP.
- File/static content handling and upload size limits are configured in the host.
- Secret values must remain outside source and documentation. Historical revisions exposed .env/configuration credentials; rotate them and keep local copies ignored.
