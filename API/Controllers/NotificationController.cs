using Application.Features.Notifications.Queries.GetNotifications;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, IMediator mediator)
        {
            _mediator = mediator;
            _notificationService = notificationService;
        }


        [HttpGet("my-notifications")]
        [Authorize]
        public async Task<IActionResult> GetNotificationByUserId([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);

            var response = await _notificationService.GetNotificationByUserIdAsync(userId, pageNumber, pageSize);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountNotification()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            var response = await _notificationService.CountNotificationAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var userId = Guid.Parse(userIdString);
            query.UserId = userId;
            var notifications = await _mediator.Send(query);
            return Ok(notifications);
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var command = new Application.Features.Notifications.Commands.MarkNotificationAsRead.MarkNotificationAsReadCommand
            {
                NotificationId = notificationId,
                UserId = Guid.Parse(userIdString)
            };

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.error);
            }
            return NoContent();
        }
    }
}
