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
            var response = await _notificationService.CountNotificationAsync(CurrentUserId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<List<NotificationDTO>> GetNotifications([FromQuery] GetNotificationsQuery query)
        {
            query.UserId = CurrentUserId;
            return await Mediator.Send(query);
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
