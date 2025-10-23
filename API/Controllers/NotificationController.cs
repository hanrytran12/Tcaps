using Application.Features.Notifications.Queries.GetNotifications;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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


        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetNotificationByUserId(Guid userId,
            [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var response = await _notificationService.GetNotificationByUserIdAsync(userId, pageNumber, pageSize);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("count/{userId:guid}")]
        public async Task<IActionResult> CountNotification(Guid userId)
        {
            var response = await _notificationService.CountNotificationAsync(userId);
            return StatusCode(response.StatusCode, response);

        }


        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query)
        {
            query.UserId = Guid.Parse("A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D");
            var notifications = await _mediator.Send(query);
            return Ok(notifications);
        }

        //[HttpGet("{userId:guid}")]
        //public async Task<IActionResult> GetNotificationByUserId(Guid userId)
        //{
        //    var response = await _notificationService.GetNotificationByUserIdAsync(userId);
        //    return StatusCode(response.StatusCode, response);
        //}

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var command = new Application.Features.Notifications.Commands.MarkNotificationAsRead.MarkNotificationAsReadCommand
            {
                NotificationId = notificationId,
                UserId = Guid.Parse("A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D")
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.error);
            }
            return NoContent();
        }

        //[HttpPut("is-read/{notificationId:guid}")]
        //public async Task<IActionResult> MarkAsRead(Guid notificationId)
        //{
        //    var response = await _notificationService.MarkAsReadAsync(notificationId);
        //    return StatusCode(response.StatusCode, response);
        //}
    }
}
