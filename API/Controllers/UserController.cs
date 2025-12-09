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
    public class UserController : BaseApiController
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
            return await _mediator.Send(new GetAllUserQuery());
        }

        [HttpGet("{workshopId:guid}")]
        public async Task<User> GetUserByWorkshopId(Guid workshopId)
        {
            return await _mediator.Send(new GetUserByWorkshopIdQuery(workshopId));
        }

        [HttpGet("{workshopId:guid}/users-in-workshop")]
        public async Task<List<UsersDTO>> GetStaffByWorkshopId(Guid workshopId)
        {
            var query = new GetStaffByWorkshopIdQuery(workshopId);
            return await _mediator.Send(query);
        }

        [HttpGet("staff-performance")]
        [Authorize(Roles = "Admin")]
        public async Task<List<StaffPerformanceDTO>> GetStaffPerformance([FromQuery] GetStaffPerformanceQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("staff-dashboard/{assignId}")]
        public async Task<StaffDashboardDTO> GetStaffDashboard(Guid assignId)
        {
            var query = new GetStaffDashboardQuery
            {
                StaffId = CurrentUserId,
                AssignId = assignId
            };

            return await _mediator.Send(query);
        }

        [HttpGet("group-progress")]
        public async Task<GroupProgressDTO> GetGroupProgress([FromQuery] Guid assignId)
        {
            var query = new GetGroupProgressQuery
            {
                UserId = CurrentUserId,
                AssignId = assignId
            };

            return await _mediator.Send(query);
        }

        [HttpGet("profile")]
        public async Task<UserDTO> GetProfileAsync()
        {
            var query = new GetUserByIdQuery(CurrentUserId);
            return await _mediator.Send(query);
        }

        [HttpGet("all-QCTransport")]
        [Authorize(Roles = "Lead")]
        public async Task<List<UserDTO>> GetAllQCTransportAsync()
        {
            var query = new GetAllQCTransportQuery();
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            await _mediator.Send(command);
            return Ok("Tạo User thành công");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok("Cập nhật User thành công");
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto, CancellationToken cancellationToken)
        {
            var response = await _staffService.ChangePasswordAsync(CurrentUserId, dto.CurrentPassword, dto.NewPassword, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            command.Id = CurrentUserId;
            await _mediator.Send(command);
            return Ok("Cập nhật thông tin thành công.");
        }

        [HttpPut("{userId:guid}/re-active")]
        public async Task<IActionResult> ReactiveUser(Guid userId)
        {
            await _mediator.Send(new ReactiveUserCommand(userId));
            return Ok("Kích hoạt lại User thành công");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return Ok("Xóa User thành công");
        }
    }
}
