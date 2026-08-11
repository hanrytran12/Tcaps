using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.ReactiveUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Commands.UpdateUserProfile;
using Application.Features.Users.Queries.GetAllLead;
using Application.Features.Users.Queries.GetAllQCTransport;
using Application.Features.Users.Queries.GetAllUser;
using Application.Features.Users.Queries.GetGroupProgress;
using Application.Features.Users.Queries.GetStaffByWorkshopId;
using Application.Features.Users.Queries.GetStaffDashboard;
using Application.Features.Users.Queries.GetStaffPerformance;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.GetUserByWorkshopId;
using Application.Features.Users.Queries.GetUserProfileById;
using Application.Interfaces;
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
        private readonly IStaffService _staffService;

        public UserController(ISender mediator, IStaffService staffService) : base(mediator)
        {
            _staffService = staffService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<UsersDTO>> GetAllUser()
        {
            return await Mediator.Send(new GetAllUserQuery());
        }

        [HttpGet("{workshopId:guid}")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<UserDTO> GetUserByWorkshopId(Guid workshopId)
        {
            return await Mediator.Send(new GetUserByWorkshopIdQuery(workshopId));
        }

        [HttpGet("{workshopId:guid}/users-in-workshop")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<List<UsersDTO>> GetStaffByWorkshopId(Guid workshopId)
        {
            return await Mediator.Send(new GetStaffByWorkshopIdQuery(workshopId));
        }

        [HttpGet("staff-performance")]
        [Authorize(Roles = "Admin")]
        public async Task<List<StaffPerformanceDTO>> GetStaffPerformance([FromQuery] GetStaffPerformanceQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("staff-dashboard/{assignId}")]
        [Authorize(Roles = "Staff")]
        public async Task<StaffDashboardDTO> GetStaffDashboard(Guid assignId)
        {
            var query = new GetStaffDashboardQuery
            {
                StaffId = CurrentUserId,
                AssignId = assignId
            };

            return await Mediator.Send(query);
        }

        [HttpGet("group-progress")]
        [Authorize(Roles = "Staff")]
        public async Task<GroupProgressDTO> GetGroupProgress([FromQuery] Guid assignId)
        {
            var query = new GetGroupProgressQuery
            {
                UserId = CurrentUserId,
                AssignId = assignId
            };

            return await Mediator.Send(query);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<UserDTO> GetProfileAsync()
        {
            var query = new GetUserByIdQuery(CurrentUserId);
            return await Mediator.Send(query);
        }

        [HttpGet("all-QCTransport")]
        [Authorize(Roles = "Lead")]
        public async Task<List<UserDTO>> GetAllQCTransportAsync()
        {
            var query = new GetAllQCTransportQuery();
            return await Mediator.Send(query);
        }

        [HttpGet("all-Lead")]
        [Authorize(Roles = "Admin")]
        public async Task<List<UserDTO>> GetAllLeadAsync()
        {
            var query = new GetAllLeadQuery();
            return await Mediator.Send(query);
        }

        [HttpGet("by-userId")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<UserDTO> GetByUserIdAsync([FromQuery] GetUserProfileByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            await Mediator.Send(command);
            return Ok("Tạo User thành công");
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            await Mediator.Send(command);
            return Ok("Cập nhật User thành công");
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto, CancellationToken cancellationToken)
        {
            var response = await _staffService.ChangePasswordAsync(CurrentUserId, dto.CurrentPassword, dto.NewPassword, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            command.Id = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Cập nhật thông tin thành công.");
        }

        [HttpPut("{userId:guid}/re-active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReactiveUser(Guid userId)
        {
            await Mediator.Send(new ReactiveUserCommand(userId));
            return Ok("Kích hoạt lại User thành công");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await Mediator.Send(new DeleteUserCommand(id));
            return Ok("Xóa User thành công");
        }
    }
}
