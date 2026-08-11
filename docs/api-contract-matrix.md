# BE API Contract Matrix

> Status: current API refactor verification matrix.
>
> Last verified: 2026-08-11 against `master`.

This document records the current HTTP-facing contract of `D:\Clone\Tcaps\Tcaps\API`. `UNMARKED` means the action has no explicit `[Authorize]` or `[AllowAnonymous]` marker at source level. It is not an approval that the endpoint should be public. The current matrix has no remaining `UNMARKED` rows after the authorization pass.

## Conventions

- `body`, `form`, `query`, `route`, `inferred`, and `none` describe the current request binding.
- Direct `IActionResult` actions commonly return `200 OK` string messages and rely on the global exception handler for errors.
- Typed actions commonly return `200 OK` with the declared type and rely on the global exception handler for errors.
- Current routes are preserved during the first refactor release because `TCaps-Mobile-FE` calls many of them directly.

## Phase 2 authorization changes

- `API/Program.cs` now uses a fallback policy requiring an authenticated user for endpoints without explicit authorization metadata.
- `AuthController` explicitly marks login, registration, forgot-password, OTP verification, and reset-password as `[AllowAnonymous]`.
- Forgot-password, OTP verification, and reset-password use the existing `OtpPolicy` rate limiter.
- `TaskTransferRequestController.GetByQcTransportAsync` now uses `Policy=QCTransportOnly`; its previous authorization marker was commented out.
- The initial fallback policy is not treated as sufficient authorization. Sensitive user, inventory, production, component-defect, assignment, and rework actions now have explicit role/policy markers.
- `User.UpdateUser`, `User.ReactiveUser`, and `User.DeleteUser` are restricted to Admin to prevent privilege escalation.
- QCK is normalized as a role policy (`RequireRole("QCK")`); QCTransport still uses its role plus the existing `isQcTransport=true` assertion where required. Representative-token verification and automated authorization tests remain pending.

## Controller actions

| Controller | Action | Verb and route | Binding | Current response | Current auth marker |
|---|---|---|---|---|---|
| Assignment | `GetAllocatedMaterials` | GET `api/Assignment/{assignmentId:guid}/allocated-materials` | inferred | `List<AllocatedMaterialDto>` | `Roles=Admin,Lead,QC,QCK,Staff` |
| Assignment | `GetAssignmentsForStaffById` | GET `api/Assignment/for-staff` | none | `List<AssignForStaffDTO>` | `Roles=Staff` |
| Assignment | `GetAssignmentForQCIdAsync` | GET `api/Assignment/qc/assignments` | none | `List<AssignForStaffDTO>` | `Policy=QC` |
| Assignment | `GetAssignmentByBatchIdAsync` | GET `api/Assignment/staff/{batchId}` | inferred | `AssignForStaffDTO` | `Roles=Staff` |
| Assignment | `GetAssignmentHistoryForQCAsync` | GET `api/Assignment/qc-lead-admin/assign-history/{batchId}` | inferred | `List<AssignmentHistoryDTO>` | `Roles=QC,QCK,Admin,Lead` |
| Assignment | `GetDetailAssignmentForQCAsync` | GET `api/Assignment/qc/detail-assignment/{batchId}` | inferred | `List<DashboardAssignmentDTO>` | `Policy=QC` |
| Assignment | `GetTaskProgressByQcIdAsync` | GET `api/Assignment/qc-staff/task-progress` | none | `List<TaskProgressDTO>` | `Policy=QC` |
| Assignment | `PlanAssignments` | POST `api/Assignment/{batchId:guid}/plan-assignments` | body | `IActionResult` | `Roles=Lead,Admin` |
| Assignment | `UpdateReadyForTransfer` | PUT `api/Assignment/update-ready-for-transfer` | query | `IActionResult` | `Policy=QC` |
| AssignmentTransferRequest | `GetAllTrasnferRequest` | GET `api/AssignmentTransferRequest` | none | `ActionResult<List<AssignmentTransferRequestDTO>>` | `Roles=Lead` |
| AssignmentTransferRequest | `GetReconciliationSummary` | GET `api/AssignmentTransferRequest/{assignmentId:guid}/reconcilliation-summary` | inferred | `ReconcilationSummaryDTO` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| AssignmentTransferRequest | `GetTransferRequestByAssignmentId` | GET `api/AssignmentTransferRequest/by-assignment/{assignmentId:guid}` | inferred | `TransferRequestDTO` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| AssignmentTransferRequest | `GetForQCTransportAsync` | GET `api/AssignmentTransferRequest/qc-transport` | query | `AssignmentTransferRequestDTO` | `Policy=QCTransportOnly` |
| AssignmentTransferRequest | `GetAllForQcTransport` | GET `api/AssignmentTransferRequest/getAll-for-qcTransport` | none | `List<AssignmentTransferRequestDTO>` | `Policy=QCTransportOnly` |
| AssignmentTransferRequest | `CreateTransferRequest` | POST `api/AssignmentTransferRequest` | body | `IActionResult` | `Roles=QC,QCK` |
| AssignmentTransferRequest | `ApproveTrasnferRequest` | PUT `api/AssignmentTransferRequest/approved/{transferRequestId:guid}` | inferred | `IActionResult` | `Policy=LeadOrValidQCTransport` |
| AssignmentTransferRequest | `QCTransportReception` | PUT `api/AssignmentTransferRequest/qc-transport-reception` | query | `IActionResult` | `Policy=QCTransportOnly` |
| Auth | `LoginAsync` | GET `api/Auth` | query | `AuthRepsponseDTO` | `[AllowAnonymous]` |
| Auth | `RegisterAsync` | POST `api/Auth/register` | body | `ActionResult<AuthRepsponseDTO>` | `[AllowAnonymous]` |
| Auth | `ForgotPassword` | POST `api/Auth/forgot-password` | body | `IActionResult` | `[AllowAnonymous],RateLimit=OtpPolicy` |
| Auth | `VerifyOtp` | POST `api/Auth/verify-otp` | body | `IActionResult` | `[AllowAnonymous],RateLimit=OtpPolicy` |
| Auth | `ResetPassword` | POST `api/Auth/reset-password` | body | `IActionResult` | `[AllowAnonymous],RateLimit=OtpPolicy` |
| Batch | `GetAllBatch` | GET `api/Batch` | none | `List<Batch>` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| Batch | `GetBatchForManagement` | GET `api/Batch/management` | none | `List<BatchDTO>` | `Roles=Admin,Lead` |
| Batch | `GetBatchById` | GET `api/Batch/{batchId:guid}` | inferred | `BatchDetailResponseDTO` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| Batch | `GetDashboardStats` | GET `api/Batch/dashboard` | query | `DashboardResultDTO` | `Policy=CanViewDashboard` |
| Batch | `GetBatchByWorkshopId` | GET `api/Batch/for-qc` | query | `List<BatchDTO>` | `Policy=QC` |
| Batch | `GetBatchesByStaffIdAsync` | GET `api/Batch/staff/batches` | none | `List<StaffSummaryDashboardDTO>` | `Roles=Staff` |
| Batch | `GetBatchesByQCIdAsync` | GET `api/Batch/qc/batches` | none | `List<BatchForQCDTO>` | `Roles=QC,QCK` |
| Batch | `GetBatchesByLeadIdAsync` | GET `api/Batch/lead/batches` | none | `List<Batch>` | `Policy=Lead` |
| Batch | `AddBatch` | POST `api/Batch` | inferred | `IActionResult` | `Policy=Admin` |
| Batch | `UpdateBatch` | PUT `api/Batch/{id:guid}` | body | `IActionResult` | `Policy=Admin` |
| Batch | `UpdateLeadForBatch` | PUT `api/Batch/lead-for-batch` | query | `IActionResult` | `Roles=Admin` |
| Batch | `DeleteBatch` | DELETE `api/Batch/{id:guid}` | inferred | `IActionResult` | `Policy=Admin` |
| ComponentDefect | `GetAllByEvaluateIdForStaffAsync` | GET `api/ComponentDefect/for-staff` | query | `List<ComponentDefectsDTO>` | `Roles=Staff` |
| ComponentDefect | `GetAllByEvaluateIdForQCAsync` | GET `api/ComponentDefect/for-qc` | query | `List<ComponentDefectsDTO>` | `Policy=QC` |
| ComponentDefect | `UpdateResolveAsync` | PUT `api/ComponentDefect/resolve/{componentId}` | query | `IActionResult` | `Roles=Staff` |
| ComponentDefect | `UpdateConfirmAsync` | PUT `api/ComponentDefect/confirm/{componentId}` | query | `IActionResult` | `Policy=QC` |
| ComponentDefect | `RejectComponentAsync` | PUT `api/ComponentDefect/reject/{componentId}` | query | `IActionResult` | `Policy=QC` |
| Evaluate | `GetAll` | GET `api/Evaluate` | none | `List<EvaluateDTO>` | `Roles=Admin,Lead,QC,QCK,Staff` |
| Evaluate | `GetByQCId` | GET `api/Evaluate/for-qc` | query | `List<EvaluateDTO>` | `Policy=QC` |
| Evaluate | `GetByStaffId` | GET `api/Evaluate/for-staff` | query | `List<EvaluateDTO>` | `Roles=Staff` |
| Evaluate | `CreateEvaluate` | POST `api/Evaluate` | form | `IActionResult` | `Policy=QC` |
| FinalTransferRequest | `GetAllAsync` | GET `api/FinalTransferRequest/all` | none | `List<FinalTransferRequestDTO>` | `Roles=GuardQC` |
| FinalTransferRequest | `UpdateApproveAsync` | PUT `api/FinalTransferRequest/approve-finalTransfer` | query | `IActionResult` | `Roles=GuardQC` |
| Income | `GetIncomesByStaffId` | GET `api/Income/by-staff` | query | `List<IncomeHistoryDTO>` | `Roles=Staff` |
| Income | `GetMonthlyIncome` | GET `api/Income/total-monthly` | query | `MonthlyIncomeDTO` | `Roles=Staff` |
| Income | `GetIncomeExpected` | GET `api/Income/income-expected` | none | `IncomeExpectedDTO` | `Roles=Staff` |
| Inventory | `GetInventoryByMaterialId` | GET `api/Inventory/from-{materialId:guid}` | query | `InventoryHistoryDTO` | `Roles=Admin,Lead,QC,QCK` |
| Inventory | `GetInventoryById` | GET `api/Inventory/{id:guid}` | inferred | `InventoryResponseDTO` | `Roles=Admin,Lead` |
| Inventory | `AddInventory` | POST `api/Inventory` | form | `IActionResult` | `Roles=Admin,Lead` |
| Material | `GetAllMaterialsAsync` | GET `api/Material` | none | `List<MaterialToWatchDTO>` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| Material | `GetAllAsync` | GET `api/Material/all` | query | `List<MaterialDTO>` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| Material | `CreateMaterial` | POST `api/Material` | body | `IActionResult` | `Roles=Admin,Lead` |
| MaterialRequest | `GetAllRequest` | GET `api/MaterialRequest` | none | `IActionResult` | `Roles=Admin,Lead,QC,QCK,QCTransport,Staff` |
| MaterialRequest | `GetPendingRequests` | GET `api/MaterialRequest/pending-confirmation` | none | `ActionResult<List<PendingRequestDTO>>` | `Policy=QC` |
| MaterialRequest | `GetAllAsync` | GET `api/MaterialRequest/lead/admin/all-request` | query | `Result<List<MaterialRequestDTO>>` | `Roles=Admin,Lead` |
| MaterialRequest | `GetByQCIdAsync` | GET `api/MaterialRequest/qc/request` | query | `Result<List<MaterialRequestDTO>>` | `Policy=QC` |
| MaterialRequest | `GetRequestsForQcTransport` | GET `api/MaterialRequest/qc-transport` | query | `Result<MaterialRequestDTO>` | `Policy=QCTransportOnly` |
| MaterialRequest | `GetForAssignmentDashboard` | GET `api/MaterialRequest/assignment-dashboard` | query | `List<MaterialRequestForAssignmentDashboardDTO>` | `Roles=Lead,QC,QCK,Staff` |
| MaterialRequest | `DispatchMaterialsToAssignment` | POST `api/MaterialRequest/{assignmentId:guid}/dispatch-materials` | body | `IActionResult` | `Policy=Lead` |
| MaterialRequest | `CreateMaterailRequestAsync` | POST `api/MaterialRequest/qc/material-requests` | body | `IActionResult` | `Policy=QC` |
| MaterialRequest | `ApproveMaterialRequest` | PUT `api/MaterialRequest/approve/{id:guid}` | route | `IActionResult` | `Policy=Lead` |
| MaterialRequest | `ConfirmMaterialRequest` | PUT `api/MaterialRequest/confirmed/{id:guid}` | body | `IActionResult` | `Roles=QC,QCK,Lead` |
| MaterialRequest | `RejectMaterialRequest` | PUT `api/MaterialRequest/rejected/{id:guid}` | body | `IActionResult` | `Policy=QC` |
| MaterialRequest | `QcTransportReceptionMaterialRequest` | PUT `api/MaterialRequest/qc-transport-reception` | query | `IActionResult` | `Policy=QCTransportOnly` |
| MaterialRequest | `LeadConfirm` | PUT `api/MaterialRequest/lead-confirm` | query | `IActionResult` | `Roles=Lead` |
| MaterialSupply | `GetAllAsync` | GET `api/MaterialSupply` | query | `Result<List<MaterialSupplyDTO>>` | `Roles=Admin,Lead,QC,QCK,QCTransport` |
| MaterialSupply | `CreateAsync` | POST `api/MaterialSupply` | body | `IActionResult` | `Roles=Lead` |
| MaterialSupply | `UpdateInProgressAsync` | PUT `api/MaterialSupply/qcTransport/InProgress/{supplyId}` | inferred | `IActionResult` | `Policy=QCTransportOnly` |
| MaterialSupply | `UpdateCompletedAsync` | PUT `api/MaterialSupply/qc/Completed/{supplyId}` | inferred | `IActionResult` | `Policy=QC` |
| MaterialSupply | `ApproveByAdminAsync` | PUT `api/MaterialSupply/admin/Approve/{supplyId}` | inferred | `IActionResult` | `Roles=Admin` |
| MaterialUse | `GetByAssignIdAsync` | GET `api/MaterialUse/qc/materials/request` | query | `List<MaterialUseDTO>` | `Policy=QC` |
| MaterialWorkshop | `GetAllAsync` | GET `api/MaterialWorkshop/all` | query | `List<MaterialWorkshopSummaryDTO>` | `Roles=Admin,Lead,QC,QCK` |
| MaterialWorkshop | `GetByQCIdAsync` | GET `api/MaterialWorkshop/for-qc` | query | `List<MaterialWorkshopDTO>` | `Policy=QC` |
| MaterialWorkshop | `GetTotalQuantityReceive` | GET `api/MaterialWorkshop/total-quantity-receive` | query | `int` | `Roles=QC,QCK,Lead` |
| MaterialWorkshop | `UpdateConfirmAsync` | PUT `api/MaterialWorkshop/update-confirm` | query | `IActionResult` | `Policy=QC` |
| Notification | `CountNotification` | GET `api/Notification/count` | none | `IActionResult` | `[Authorize]` |
| Notification | `GetNotifications` | GET `api/Notification` | query | `List<NotificationDTO>` | `[Authorize]` |
| Notification | `MarkAsRead` | PUT `api/Notification/mark-as-read/{notificationId}` | inferred | `IActionResult` | `[Authorize]` |
| Product | `GetAllProduct` | GET `api/Product` | none | `List<ProductsDTO>` | `Roles=Admin,Lead,QC,QCK,Staff` |
| Product | `AddProduct` | POST `api/Product` | form | `IActionResult` | `Policy=Admin` |
| Product | `UpdateProduct` | PUT `api/Product/{id:guid}` | form | `IActionResult` | `Policy=Admin` |
| Product | `DeleteProduct` | DELETE `api/Product/{id:guid}` | inferred | `IActionResult` | `Policy=Admin` |
| Production | `GetAllAsync` | GET `api/production/all` | none | `List<ProductionDTO>` | `Roles=Admin,Lead,QC,QCK` |
| Production | `GetByStaffIdAsync` | GET `api/production/for-staff` | query | `List<ProductionDTO>` | `Roles=Staff` |
| Production | `GetProductionsWithStatusPendingQC` | GET `api/production/for-qc` | query | `List<ProductionDTO>` | `Policy=QC` |
| Production | `GetProductionByAssignIdAsync` | GET `api/production/by-assignId` | query | `List<ProductionDTO>` | `Roles=Staff` |
| Production | `ReportWork` | POST `api/production/report-work` | body | `IActionResult` | `Roles=Staff` |
| Production | `NotifyMaterialShortage` | POST `api/production/notify-material-shortage` | body | `IActionResult` | `Roles=Staff` |
| Production | `UpdateQuantity` | PUT `api/production/for-qc/reduce-quantity` | query | `IActionResult` | `Policy=QC` |
| QC | `GetAllComponentDefect` | GET `api/QC/rework-requests` | query | `List<ComponentDefectsDTO>` | `Policy=QC` (class) |
| ReworkRequest | `GetAllReworkRequest` | GET `api/ReworkRequest` | none | `List<ReworkRequestDTO>` | `Policy=Lead` |
| ReworkRequest | `GetReworkRequestById` | GET `api/ReworkRequest/{reworkRequestId:guid}` | inferred | `ReworkRequestResponseDTO` | `Roles=Lead,QC,QCK,Staff` |
| ReworkRequest | `GetReworkReconciliationSummary` | GET `api/ReworkRequest/{assignmentId:guid}/summary` | inferred | `ReconcilationSummaryDTO` | `Roles=Lead,QC,QCK,Staff` |
| ReworkRequest | `GetByAssignId` | GET `api/ReworkRequest/by-assignId` | query | `ReworkRequestResponseDTO` | `Roles=Lead,QC,QCK,Staff` |
| ReworkRequest | `GetReworkForDashboard` | GET `api/ReworkRequest/{assignId:guid}/for-dashboard` | inferred | `ReworkRequestDTO` | `Roles=Lead,QC,QCK,Staff` |
| ReworkRequest | `GetReworkByQcId` | GET `api/ReworkRequest/by-qc` | none | `List<ReworkRequestDTO>` | `Policy=QC` |
| ReworkRequest | `CreateReworkRequest` | POST `api/ReworkRequest` | body | `IActionResult` | `Policy=QC` |
| ReworkRequest | `RejectReworkRequest` | PUT `api/ReworkRequest/{requestId:guid}/rejected` | inferred | `IActionResult` | `Policy=Lead` |
| ReworkRequest | `ApproveReworkRequest` | PUT `api/ReworkRequest/{requestId:guid}/approved` | body | `IActionResult` | `Policy=Lead` |
| TaskTransferRequest | `GetAllAsync` | GET `api/TaskTransferRequest/all` | query | `List<TaskTransferRequestDTO>` | `Roles=Admin,Lead` |
| TaskTransferRequest | `GetById` | GET `api/TaskTransferRequest/materialRequestId-assignmentTransferId` | query | `TaskTransferRequestDTO` | `Roles=Admin,Lead,QC,QCK,QCTransport` |
| TaskTransferRequest | `GetByQcTransportAsync` | GET `api/TaskTransferRequest/for-QcTransport` | query | `List<TaskTransferRequestDTO>` | `Policy=QCTransportOnly` |
| TaskTransferRequest | `CreateAsync` | POST `api/TaskTransferRequest/for-lead` | body | `IActionResult` | `Roles=Lead` |
| TaskTransferRequest | `ApproveRequestAsync` | PUT `api/TaskTransferRequest/approved` | query | `IActionResult` | `Roles=Admin` |
| User | `GetAllUser` | GET `api/User` | none | `List<UsersDTO>` | `Roles=Admin` |
| User | `GetUserByWorkshopId` | GET `api/User/{workshopId:guid}` | inferred | `UserDTO` | `Roles=Admin,Lead` |
| User | `GetStaffByWorkshopId` | GET `api/User/{workshopId:guid}/users-in-workshop` | inferred | `List<UsersDTO>` | `Roles=Admin,Lead` |
| User | `GetStaffPerformance` | GET `api/User/staff-performance` | query | `List<StaffPerformanceDTO>` | `Roles=Admin` |
| User | `GetStaffDashboard` | GET `api/User/staff-dashboard/{assignId}` | inferred | `StaffDashboardDTO` | `Roles=Staff` |
| User | `GetGroupProgress` | GET `api/User/group-progress` | query | `GroupProgressDTO` | `Roles=Staff` |
| User | `GetProfileAsync` | GET `api/User/profile` | none | `UserDTO` | `[Authorize]` |
| User | `GetAllQCTransportAsync` | GET `api/User/all-QCTransport` | none | `List<UserDTO>` | `Roles=Lead` |
| User | `GetAllLeadAsync` | GET `api/User/all-Lead` | none | `List<UserDTO>` | `Roles=Admin` |
| User | `GetByUserIdAsync` | GET `api/User/by-userId` | query | `UserDTO` | `Roles=Admin,Lead` |
| User | `AddUser` | POST `api/User` | body | `IActionResult` | `Roles=Admin` |
| User | `UpdateUser` | PUT `api/User/{id:guid}` | body | `IActionResult` | `Roles=Admin` |
| User | `ChangePassword` | PUT `api/User/change-password` | body | `IActionResult` | `[Authorize]` |
| User | `UpdateProfile` | PUT `api/User/update-profile` | body | `IActionResult` | `[Authorize]` |
| User | `ReactiveUser` | PUT `api/User/{userId:guid}/re-active` | inferred | `IActionResult` | `Roles=Admin` |
| User | `DeleteUser` | DELETE `api/User/{id:guid}` | inferred | `IActionResult` | `Roles=Admin` |
| Workshop | `GetWorkshopsTemplate` | GET `api/Workshop` | none | `List<WorkshopsDTO>` | `Roles=Admin,Lead,QC,QCK,Staff` |
| Workshop | `AddWorkshop` | POST `api/Workshop` | body | `IActionResult` | `Roles=Admin` |
| Workshop | `InsertWorkshopAsync` | PUT `api/Workshop/insert` | query | `IActionResult` | `Roles=Admin` |
| Workshop | `UpdateAsync` | PUT `api/Workshop/update` | query | `IActionResult` | `Roles=Admin` |
| Workshop | `SwapAsync` | PUT `api/Workshop/swap-workshop` | query | `IActionResult` | `Roles=Admin` |
| Workshop | `DeleteAsync` | DELETE `api/Workshop` | query | `IActionResult` | `Roles=Admin` |
| WorkshopInventory | `GetAllWorkshopInventory` | GET `api/WorkshopInventory` | none | `List<WorkshopInventoryDTO>` | `Roles=Admin,Lead,QC,QCK` |
| WorkshopInventory | `GetWorkshopInvenntoryByWorkshopId` | GET `api/WorkshopInventory/{workshopId:guid}` | inferred | `List<WorkshopInventoryForExportDTO>` | `Roles=Admin,Lead,QC,QCK,QCTransport` |
| WorkshopInventory | `GetWorkshopInventoryForQC` | GET `api/WorkshopInventory/for-qc` | none | `List<WorkshopInventoryForQCDTO>` | `Policy=QC` |
| WorkshopInventory | `GetByMaterialId` | GET `api/WorkshopInventory/by-material` | query | `WorkshopInventoryDTO` | `Roles=Admin,Lead,QC,QCK,Staff` |

## Mobile consumer index

These are the consumer files found during the baseline scan. This is not a claim that no other consumer exists.

| API area | Mobile consumer files |
|---|---|
| Auth | `D:\Clone\Tcaps\TCaps-Mobile-FE\app\config\api.ts` |
| Assignment | `app/services/assignment-service.ts`, `app/services/production.service.ts` |
| Assignment transfer | `app/services/assignment-transfer-service.ts` |
| Batch | `app/services/batch-service.ts`, `app/hooks/use-batch-queries.ts`, `app/services/production.service.ts`, `app/(tabs)/request.tsx` |
| Income | `app/services/income-service.ts` |
| Material request/supply/workshop/use | `app/services/material-request-service.ts`, `app/services/material-request.service.ts`, `app/services/material-supply-service.ts`, `app/services/material-workshop-service.ts`, `app/services/inventory-service.ts`, `app/services/production.service.ts` |
| Product | `app/services/product-service.ts` |
| Production/evaluate | `app/services/production.service.ts` |
| Notifications | `app/services/production.service.ts` |
| Rework | `app/services/rework-request-service.ts` |
| User | `app/services/user-service.ts`, `app/services/production.service.ts` |
| Workshop/inventory | `app/services/workshop-service.ts`, `app/services/workshop-inventory-service.ts`, `app/services/inventory-service.ts` |

## Refactor decisions

1. Treat `UNMARKED` business actions as protected by default in Phase 2 until a product owner confirms a public use case.
2. Add explicit `[AllowAnonymous]` to the login and password-recovery flow instead of relying on an implicit anonymous default.
3. Preserve route strings such as `reconcilliation-summary`, `all-QCTransport`, and `for-QcTransport` during the compatibility window.
4. Replace direct Domain Entity responses with DTOs feature by feature.
5. Do not introduce a universal response envelope until the mobile client has an approved migration plan.

## Phase 4 contract changes

- `GET api/User/{workshopId:guid}` now returns `UserDTO`, excluding `PasswordHash` and other domain-only fields.
- `POST api/User` and `PUT api/User/change-password` explicitly bind JSON request bodies; route strings and success payloads are unchanged.
- WorkshopInventory, Income, and MaterialWorkshop response boundaries now use DTOs while preserving the mobile-facing routes and scalar fields.
- Batch/Inventory/Rework entity responses remain compatibility-sensitive and are scheduled for feature-by-feature DTO migration.

## Known contract issues

- The mobile client references legacy route spellings and some route names that do not exactly match current controller attributes. These must be verified per flow before renaming.
- `Auth.LoginAsync` uses `GET` with query-bound credentials; changing it to `POST` is a breaking change and requires a compatibility plan.
- Several write actions use query-bound command objects or primitive parameters. Normalize only after the mobile caller is migrated.
- Authorization markers reflect the current source metadata; role/claim semantics still require integration coverage.
