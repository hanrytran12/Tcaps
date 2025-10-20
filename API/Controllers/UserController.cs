using Application.DTOs;
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

        [HttpGet("total-income/{userId:guid}")]
        public async Task<IActionResult> TotalIncome(Guid userId)
        {
            var response = await _staffService.GetTotalIncomeAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("history-income/{userId:guid}")]
        public async Task<IActionResult> HistoryIncome(Guid userId)
        {
            var response = await _staffService.GetIncomeHistoryAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("assignment/{userId:guid}")]
        public async Task<IActionResult> GetAssignment(Guid userId)
        {
            var response = await _staffService.GetAssignmentsAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("change-password/{userId:guid}")]
        public async Task<IActionResult> ChangePassword(Guid userId, ChangePasswordDTO dto, CancellationToken cancellationToken)
        {
            var response = await _staffService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-profile/{userId:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid userId, UpdateProfileUserDTO dto, CancellationToken cancellationToken)
        {
            var response = await _staffService.UpdateProfileAsync(userId, dto, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }
}
