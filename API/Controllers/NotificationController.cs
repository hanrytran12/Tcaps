using Application.DTOs.Response;
using Application.Features.Notifications.Queries.GetNotifications;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, ISender mediator) : base(mediator)
        {
            _notificationService = notificationService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountNotification()
        {
            var result = await _notificationService.CountNotificationAsync(CurrentUserId);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationDTO>>> GetNotifications([FromQuery] GetNotificationsQuery query)
        {
            query.UserId = CurrentUserId;
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var result = await Mediator.Send(new Application.Features.Notifications.Commands.MarkNotificationAsRead.MarkNotificationAsReadCommand
            {
                NotificationId = notificationId,
                UserId = CurrentUserId
            });
            return HandleResult(result, "Notification marked as read successfully.");
        }
    }
}
