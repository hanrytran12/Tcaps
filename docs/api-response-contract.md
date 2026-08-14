# API response contract

The API boundary now has one failure shape and two success conventions.

## Success

- Read actions return the declared DTO, list, or scalar directly with `200 OK`.
- Write actions return `ApiMessageResponse` (`{ "message": "..." }`) with
  `200 OK`, or `204 No Content` when no message is required.
- Login keeps its existing `AuthResponseDTO` (`token`, `expiresAt`) because it
  is an authentication payload rather than a generic result envelope.

## Failure

Application handlers return `Result` or `Result<T>`. They do not reference HTTP
status codes. `API/Mappings/ApiResultMapper.cs` is the only boundary that maps
the result type to HTTP:

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
not found `404`, conflict `409`, and internal failure `500`.

## FE consumption

The mobile service layer returns its internal `ApiResponse<T>` (`success`,
`data`, `message`, `error`). Axios errors are normalized by
`app/utils/api-helper.ts`. `LegacyApiResponse<T>` and its adapter are temporary
compatibility code for old deployments; new services must not add endpoint-level
checks for `isSuccess` or `value`.

