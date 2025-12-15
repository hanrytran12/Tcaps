using Application.DTOs.Response;
using Application.Features.Notifications.Queries.GetNotifications;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, IMediator mediator)
        {
            _mediator = mediator;
            _notificationService = notificationService;
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
        public async Task<List<NotificationDTO>> GetNotifications([FromQuery] GetNotificationsQuery query)
        {
            query.UserId = CurrentUserId;
            return await _mediator.Send(query);
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            await Mediator.Send(new Application.Features.Notifications.Commands.MarkNotificationAsRead.MarkNotificationAsReadCommand
            {
                NotificationId = notificationId,
                UserId = CurrentUserId
            });
            return Ok("Notification marked as read successfully.");
        }
    }
}
