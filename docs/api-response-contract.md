# API response contract

> Last verified: 2026-08-15 against `refactor/api-contract-sync`.

The API boundary separates Application return types from HTTP wire responses.
Application does not know HTTP status codes. Controllers either return direct
DTO/list/scalar values or pass `Result`/`Result<T>` through the API mapper.

## Request flow

```text
Controller -> MediatR -> Application handler
                         ├─ DTO/list/scalar -> controller Ok(...)
                         ├─ Result/Result<T> -> ApiResultMapper
                         └─ exception -> GlobalExceptionHandler
```

## Successful responses

### Reads

- Queries returning DTOs, lists, or scalars return the declared value directly
  with `200 OK`.
- Queries returning `Result<T>` also unwrap `Value` on success and return it
  directly with `200 OK`.
- An empty collection is a valid `200 OK` response when the query returns a
  list directly.

### Writes and authentication exceptions

`ApiResultMapper` in `API/Mappings/ApiResultMapper.cs` applies this table:

| Application result | HTTP success response |
|---|---|
| `Result<T>` without success message | `200 OK` with `result.Value` |
| `Result<T>` with success message | `200 OK` with `{ "message": "..." }` |
| `Result` without success message | `204 No Content` |
| `Result` with success message | `200 OK` with `{ "message": "..." }` |

Authentication endpoints have explicit payloads:

- Login returns `AuthResponseDTO` (`token`, `expiresAt`) directly.
- Registration returns `Result<AuthResponseDTO>` through the mapper and
  exposes the successful `AuthResponseDTO` value.
- OTP verification returns `ApiTokenResponse` (`message`, `token`).

## Failure responses

Application result failures are mapped by `ApiResultMapper` and the shared
`API/Mappings/ApiErrorResponseFactory.cs`. The global handler is the safety net
for exceptions that escape the controller path. A small legacy branch in
`AuthController` constructs the same error contract for email-delivery failure.

```json
{
  "code": "resource_not_found",
  "message": "Resource was not found.",
  "status": 404,
  "traceId": "00-...",
  "errors": null
}
```

The standard mapping is validation `400`, unauthorized `401`, forbidden `403`,
not found `404`, conflict `409`, and internal failure `500`. Raw exception
messages, stack traces, credentials, and provider details are not client data.

## FE consumption

The mobile service layer returns its internal `ApiResponse<T>` (`success`,
`data`, `message`, `error`). Axios errors are normalized by
`TCaps-Mobile-FE/app/utils/api-helper.ts`. `LegacyApiResponse<T>` and its
adapter are compatibility code for old deployments; new services must not add
endpoint-level checks for `isSuccess` or `value`.
