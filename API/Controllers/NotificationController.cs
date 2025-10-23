using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
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

        [HttpPut("is-read/{notificationId:guid}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var response = await _notificationService.MarkAsReadAsync(notificationId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
