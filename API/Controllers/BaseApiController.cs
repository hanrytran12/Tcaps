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
    }
}
