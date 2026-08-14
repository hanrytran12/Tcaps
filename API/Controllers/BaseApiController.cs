using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected BaseApiController(ISender mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected ISender Mediator { get; }

        protected Guid CurrentUserId
        {
            get
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    throw new UnauthorizedAccessException("Thông tin định danh người dùng không hợp lệ.");
                }

                return userId;
            }
        }

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess) return Ok(result.Value);
            return MapFailure(result.ErrorType, result.Error);
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess) return Ok();
            return MapFailure(result.ErrorType, result.Error);
        }

        private IActionResult MapFailure(ResultErrorType? errorType, string? error)
        {
            var payload = new { error };

            return errorType switch
            {
                ResultErrorType.NotFound => NotFound(payload),
                ResultErrorType.Conflict => Conflict(payload),
                _ => BadRequest(payload)
            };
        }
    }
}
