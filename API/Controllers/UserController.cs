using Application.Commands.AddUser;
using Application.Commands.DeleteUser;
using Application.Commands.UpdateUser;
using Application.Queries.GetAllUser;
using Domain.Entities;
using MediatR;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
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
