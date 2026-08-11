using System.ComponentModel;
using Application.DTOs.Response;
using Application.Features.FinalTransferRequest.Command.ApproveFinalTransferRequest;
using Application.Features.FinalTransferRequest.Queries.GetAllFinalTransferRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalTransferRequestController : ControllerBase
    {
        private readonly ISender _sender;

        public FinalTransferRequestController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("all")]
        [Authorize(Roles = "GuardQC")]
        public async Task<List<FinalTransferRequestDTO>> GetAllAsync()
        {
            return await _sender.Send(new GetAllFinalTransferRequestQuery());
        }

        [HttpPut("approve-finalTransfer")]
        [Authorize(Roles = "GuardQC")]
        public async Task<IActionResult> UpdateApproveAsync([FromQuery] ApproveFinalTransferRequestCommand command)
        {
            await _sender.Send(command);
            return Ok("Duyệt đơn chuyển giao cuối cùng thành công");
        }
    }
}
