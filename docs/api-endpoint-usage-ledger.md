# API endpoint usage ledger

> Last verified: 2026-08-15 against `refactor/api-contract-sync`.

The usage statuses below come from a static mobile-repository audit. They do
not prove that no web, integration, provisioning, or external client uses a
route.

This ledger records the mobile FE usage audit for the API contract refactor. A
route is not removed or disabled solely because its path is absent from the
mobile repository: the BE may have web, integration, seed, or external clients.

## Audit status

- `ACTIVE_MOBILE`: referenced by the mobile FE or required by a mobile flow.
- `UNUSED_MOBILE_EXTERNAL_UNKNOWN`: not found by static mobile search; keep the
  route and verify external consumers before deprecation or removal.
- `DUPLICATE_OR_LEGACY`: compatibility path or legacy response handling that
  should be removed only after the contract migration is deployed.

## Candidate routes requiring external-client confirmation

| Controller action | Route | Status | Evidence | Safe action now |
| --- | --- | --- | --- | --- |
| `AssignmentController.GetTaskProgressByQcIdAsync` | `GET /api/Assignment/qc-staff/task-progress` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `FinalTransferRequestController.GetAllAsync` | `GET /api/FinalTransferRequest/all` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `FinalTransferRequestController.UpdateApproveAsync` | `PUT /api/FinalTransferRequest/approve-finalTransfer` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `UserController.GetStaffPerformance` | `GET /api/User/staff-performance` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `UserController.GetAllLeadAsync` | `GET /api/User/all-Lead` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `UserController.GetByUserIdAsync` | `GET /api/User/by-userId` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `MaterialWorkshopController.GetAllAsync` | `GET /api/MaterialWorkshop/all` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `QCController.GetAllComponentDefect` | `GET /api/QC/rework-requests` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `IncomeController.GetIncomesByStaffId` | `GET /api/Income/by-staff` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No matching mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `AuthController.RegisterAsync` | `POST /api/Auth/register` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | Mobile FE currently calls login and password-recovery flows only | Keep route; register may be used by provisioning tools |
| `AuthController.ForgotPassword` | `POST /api/Auth/forgot-password` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No direct mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `AuthController.VerifyOtp` | `POST /api/Auth/verify-otp` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No direct mobile service call found in the static audit | Keep route; confirm consumers before deprecating |
| `AuthController.ResetPassword` | `POST /api/Auth/reset-password` | `UNUSED_MOBILE_EXTERNAL_UNKNOWN` | No direct mobile service call found in the static audit | Keep route; confirm consumers before deprecating |

## Deprecation rule

When an external-client owner confirms a candidate is dead, add the owner and
date to this file first. Then mark the action with an explicit deprecation
comment or attribute in an isolated commit. Removal requires a rollout note and
an agreed rollback path; it must not be bundled with response-shape changes.
