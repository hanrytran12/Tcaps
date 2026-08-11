# Code Standards

> Last verified: 2026-08-12 against the current API refactor worktree.

## Architectural standards

Use the existing layered structure:

~~~text
API → Application → Domain abstractions
                 ↘ Infrastructure implementations
~~~

The API host composes the layers. Domain should remain independent of EF Core, SQL Server, Redis, Azure, email, and ASP.NET implementation details.

## Feature implementation

For a new use case, follow the surrounding feature pattern:

1. Add a request/command/query under the relevant Application/Features area.
2. Add its handler and validator where validation is required.
3. Use existing DTOs/results or add focused DTOs under Application/DTOs.
4. Use repository or IAppDbContext abstractions rather than creating ad-hoc database access.
5. Add/update the API controller action with the narrowest authorization requirement.
6. Add domain events only when a state change needs decoupled follow-up behavior.
7. Update migrations and docs when persistence or external behavior changes.

## Naming and layout

- Types and public members: PascalCase.
- Parameters and locals: camelCase.
- One focused responsibility per class.
- Feature folders should expose intent: Commands, Queries, Events, and descriptive use-case folders.
- New filenames should be descriptive and kebab-case where documentation/scripts require it; preserve existing C# type filename conventions in source.
- Do not rename legacy public types solely for spelling without checking client and migration impact.

## Request pipeline and errors

- MediatR validation behavior runs for registered validators.
- Transaction behavior is registered by Infrastructure.
- Handlers should return existing Result/Result<T> shapes when their feature uses them.
- Let the existing exception middleware/handler translate unexpected failures consistently.
- Avoid returning raw exception details or internal connection/provider errors to clients.

## Persistence

- Keep entity rules in Domain and EF mapping in Infrastructure persistence configuration.
- Use existing unit-of-work/repository contracts.
- Make transaction boundaries explicit for multi-step state changes.
- Generate migrations with EF Core; inspect both the migration and model snapshot.
- Ensure seed operations are idempotent before running them at startup.

## API and integration standards

- Routes use api/[controller] and existing HTTP verb conventions.
- Controllers use constructor-injected `ISender`; do not resolve MediatR through `RequestServices`.
- Keep `API/Program.cs` as a small composition root; place host service registration and middleware composition in focused extension classes.
- Use explicit `[FromBody]` for complex JSON writes and `[FromQuery]` for filter/query objects when the binding is part of the contract.
- Do not return Domain entities from new API actions; use response DTOs and document any compatibility exception in the API contract matrix.
- Use existing JWT roles/policies instead of duplicating authorization logic in handlers.
- Treat Blob Storage, email, Redis, and SignalR as external boundaries with clear failure behavior.
- Review CORS, upload size limits, rate limiting, forwarded headers, and hub token handling when changing Program.cs.

## Documentation and quality

- Keep README.md concise; put detailed material in docs/.
- Never put credentials, tokens, connection strings, or copied .env values in docs or examples.
- Build the solution after source changes and run all available tests.
- Use git diff --check and review the final diff before handoff.
