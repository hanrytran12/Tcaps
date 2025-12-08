using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.ReactiveUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Commands.UpdateUserProfile;
using Application.Features.Users.Queries.GetAllQCTransport;
using Application.Features.Users.Queries.GetAllUser;
using Application.Features.Users.Queries.GetGroupProgress;
using Application.Features.Users.Queries.GetStaffByWorkshopId;
using Application.Features.Users.Queries.GetStaffDashboard;
using Application.Features.Users.Queries.GetStaffPerformance;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.GetUserByWorkshopId;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStaffService _staffService;

        public UserController(IMediator mediator, IStaffService staffService)
        {
            _mediator = mediator;
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<List<UsersDTO>> GetAllUser()
        {
            var listUser = await _mediator.Send(new GetAllUserQuery());
            return listUser;
        }

        [HttpGet("{workshopId:guid}")]
        public async Task<User> GetUserByWorkshopId(Guid workshopId)
        {
            var query = new GetUserByWorkshopIdQuery(workshopId);
            return await _mediator.Send(query);
        }

        [HttpGet("{workshopId:guid}/users-in-workshop")]
        public async Task<List<UsersDTO>> GetStaffByWorkshopId(Guid workshopId)
        {
            var query = new GetStaffByWorkshopIdQuery(workshopId);
            var result = await _mediator.Send(query);
            return result;
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

        [HttpGet("productions/{userId:guid}")]
        public async Task<IActionResult> GetProdutions(Guid userId)
        {
            var response = await _staffService.GetProductionsAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("evaluate-history/{userId:guid}")]
        public async Task<IActionResult> GetEvaluateHistory(Guid userId)
        {
            var response = await _staffService.GetEvaluateHistoryAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("staff-performance")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStaffPerformance([FromQuery] GetStaffPerformanceQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("staff-dashboard/{assignId}")]
        public async Task<IActionResult> GetStaffDashboard(Guid assignId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetStaffDashboardQuery
            {
                StaffId = Guid.Parse(userIdString),
                AssignId = assignId
            };

            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        [HttpGet("group-progress")]
        public async Task<IActionResult> GetGroupProgress([FromQuery] Guid assignId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetGroupProgressQuery
            {
                UserId = Guid.Parse(userIdString),
                AssignId = assignId
            };

            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdString);
            var query = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all-QCTransport")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> GetAllQCTransportAsync()
        {
            var query = new GetAllQCTransportQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetAllUser), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto, CancellationToken cancellationToken)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdString);
            var response = await _staffService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdString);
            command.Id = userId;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpPut("{userId:guid}/re-active")]
        public async Task<IActionResult> ReactiveUser(Guid userId)
        {
            var command = new ReactiveUserCommand { UserId = userId };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }
    }
}
