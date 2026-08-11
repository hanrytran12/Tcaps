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

### Verification

- `dotnet restore Tcaps.sln` passed.
- `dotnet build Tcaps.sln --no-restore` passed with 0 errors; legacy nullability warnings remain outside this change.
- `dotnet test Tcaps.sln --no-build` passed with 9 focused API tests.
- Static verification found no controller service-locator usage, no unmarked API matrix rows, and only `.env.example` tracked as an environment file.
- Added focused handler coverage for MaterialWorkshop status filtering, ordering, and DTO field parity.
- Installed mobile dependencies from the existing lockfile. FE typecheck remains failing on pre-existing type/model issues; lint reports 4 errors and 105 warnings. No FE source or lockfile changes were made.
