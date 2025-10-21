using Application.DTOs;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetAllUser;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<User>> GetAllUser()
        {
            var listUser = await _mediator.Send(new GetAllUserQuery());
            return listUser;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
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

        [HttpGet("staff-performance")]
        public async Task<IActionResult> GetStaffPerformance(Guid WorkshopId)
        {
            var query = new Application.Features.Users.Queries.GetStaffPerformance.GetStaffPerformanceQuery(WorkshopId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
