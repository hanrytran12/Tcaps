# Project Changelog

## 2026-08-11 — API refactor continuation

- Added the API contract matrix covering all 132 controller actions and mobile consumer references.
- Added authenticated-user fallback authorization with explicit anonymous auth and password-recovery actions.
- Activated the `QCTransportOnly` policy for task transfer requests.
- Replaced controller service location with constructor-injected `ISender` and removed duplicate `_mediator` fields.
- Removed duplicate FluentValidation exception mapping and obsolete AutoMapper/MediatR DI package references.
- Added startup validation for the required `JWT_KEY` environment variable.
- Migrated `GET /api/User/{workshopId:guid}` to a `UserDTO` projection that excludes password hashes.
- Made JSON body binding explicit for user creation, password change, and batch creation.
- Added explicit role/policy authorization to sensitive user, inventory, production, assignment, material, defect, rework, transfer, and dashboard actions.
- Restricted user administration actions to Admin, closing the privilege-escalation path where an authenticated user could update, reactivate, or delete another user.
- Recorded the current role/claim verification gap for QCK and QCTransport; automated authorization tests remain pending.
- Normalized the QCK policy to the role claim emitted by the JWT generator instead of requiring an unavailable standalone `QCK` claim.
- Refreshed README, code standards, deployment guidance, roadmap, architecture notes, and the API contract matrix for the current refactor state.
- Restored mobile-compatible read access for Lead batch distribution, QC inventory views, and QCTransport request-tab lookups without broadening protected write actions.
- Updated QC authorization to include the supported QCK role and synchronized the contract matrix with the resulting role markers.
- Migrated WorkshopInventory list and material lookup responses to `WorkshopInventoryDTO` while preserving the mobile-consumed fields and routes.
- Migrated the staff income history response to `IncomeHistoryDTO` with the legacy JSON fields preserved.
- Migrated `MaterialWorkshop/all` to `MaterialWorkshopSummaryDTO`; the QC-specific DTO route remains unchanged.
- Migrated ReworkRequest detail and assignment lookup responses to `ReworkRequestResponseDTO` while preserving the mobile-consumed scalar fields.
- Migrated the Inventory detail response to `InventoryResponseDTO` while preserving its scalar entity fields.
- Migrated Batch all and Lead lookup responses to `BatchResponseDTO` with assignment summaries, removing aggregate navigation data from the API contract.
- Extracted API service registration, host limits, middleware composition, and database initialization from `Program.cs` into focused extensions while preserving runtime order and retry behavior.

### Verification

- `dotnet restore Tcaps.sln` passed.
- `dotnet build Tcaps.sln --no-restore` passed with 0 errors; legacy nullability warnings remain outside this change.
- `dotnet test Tcaps.sln --no-build` passed with 13 focused API tests.
- Static verification found no controller service-locator usage, no unmarked API matrix rows, and only `.env.example` tracked as an environment file.
- Added focused handler coverage for MaterialWorkshop status filtering, ordering, and DTO field parity.
- Added Batch response contract coverage for field parity, navigation-data exclusion, lead filtering, route templates, and authorization metadata.
- Verified the host extraction with `dotnet build` (0 errors) and 13 passing API tests; dedicated host configuration integration coverage remains a follow-up.
- Completed a focused mobile consumer scan for the migrated DTO endpoints; no reads of removed Batch navigation data were found, while existing FE typecheck/lint failures remain unchanged.
- Installed mobile dependencies from the existing lockfile. FE typecheck remains failing on pre-existing type/model issues; lint reports 4 errors and 105 warnings. No FE source or lockfile changes were made.

## 2026-08-12 — Cross-repository contract cleanup

- Normalized FE assignment-history state and service return types to the BE list contract.
- Aligned FE material-supply creation and display with the BE `quantitySend`, `quantityReceive`, and `dateReceive` fields.
- Harmonized FE product identity, batch/sample assignment models, auth identity, theme aliases, and QCTransport route permissions.
- Fixed FE TypeScript drift and lint blockers without weakening required stock-out identifiers or adding credential material.

### Verification

- FE `npx tsc --noEmit` passed with 0 errors.
- FE `npm run lint` passed with 0 errors; 105 legacy warnings remain for a separate cleanup pass.

## 2026-08-14 — BE/FE API contract synchronization

- Added focused API metadata tests for login, command binding, and product multipart updates.
- Changed mobile login to `POST api/Auth/login` with a JSON body matching the backend `LoginQuery` contract.
- Aligned Batch, ComponentDefect, TaskTransferRequest, Workshop, and assignment-transfer write callers with explicit backend request bodies.
- Changed Batch, Product, and Workshop mobile write services to consume message-only responses without casting them to domain models.
- Made product image updates optional on the backend and preserved the existing image when no replacement file is supplied.
- Required explicit quantity and lead-note values for assignment-transfer approval callers.
- Removed debug response/payload logging from the changed assignment-transfer and batch client services.

### Verification

- `dotnet build Tcaps.sln --no-restore` passed with 0 errors.
- `dotnet test Tcaps.sln --no-restore` passed with 20 tests.
- FE `npx tsc --noEmit` passed with 0 errors.
- Targeted FE ESLint passed with 0 errors; existing warnings remain.

## 2026-08-14 — Legacy API contract cleanup

- Fixed staff/QC batch route typos and aligned QC assignment history/detail with the BE direct-array responses.
- Corrected production report-work empty-success handling and sent QC quantity reduction as the required JSON body.
- Mapped assignment-transfer send/receive quantities and lead notes to the exact BE DTO and command fields.
- Added required `dateToGo` to task-transfer creation and aligned task-transfer GET-by parsing with direct DTO responses.
- Changed material-supply and rework creation services to message-only response contracts.
- Replaced the ignored workshop `stepOrder` create field with the BE `WorkshopType` field.
- Made mobile batch-update inputs require the BE validator's complete quantity/start/end-date payload.
- Expanded API metadata tests for route templates and complex write body bindings.

## 2026-08-14 — Result contract and mobile response standardization

- Added typed `ApiErrorResponse`, `ApiMessageResponse`, and `ApiTokenResponse` contracts at the API boundary.
- Added the centralized `ApiResultMapper` for validation, authorization, not-found, conflict, and internal-error status mapping.
- Migrated controller writes and legacy password/notification service responses to Application `Result` handling.
- Added an endpoint usage ledger; mobile-unused routes remain enabled until external consumers are confirmed.
- Consolidated FE legacy response compatibility into `api-helper.ts` and removed endpoint-level wrapper parsing from migrated services.

### Verification

- `dotnet build Tcaps.sln --no-restore` passed with 0 errors.
- `dotnet test Tcaps.sln --no-restore` passed with 29 tests.
- FE `npx tsc --noEmit` passed with 0 errors.
- FE `npx expo lint` passed with 0 errors; existing warnings remain.

### Verification

- `dotnet test Tcaps.sln --no-restore` passed with 26 tests.
- FE `npx tsc --noEmit` passed with 0 errors.
- FE `npm run lint` passed with 0 errors; 100 existing/legacy warnings remain.

## 2026-08-14 — Shared API error handling

- Centralized `Result` and exception error classification in
  `ApiErrorResponseFactory` without changing the API error wire contract.
- Kept `ApiResultMapper` for expected Application `Result` failures and
  `GlobalExceptionHandler` as the exception safety net.
- Replaced raw known-exception messages with safe client-facing messages while
  retaining server-side exception logging and trace IDs.
- Added contract tests for all mapped HTTP error statuses, validation details,
  success responses, mapper invariants, and global exception sanitization.

### Verification

- `dotnet build Tcaps.sln --no-restore` passed with 0 errors.
- `dotnet test Tcaps.sln --no-restore` passed with 37 tests.
- `git diff --check` passed.
