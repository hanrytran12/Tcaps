using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public UserController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var response = await _staffService.GetUserProfileAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
