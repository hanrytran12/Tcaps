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
        private readonly IMediator _mediator;

        public FinalTransferRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<List<FinalTransferRequestDTO>> GetAllAsync()
        {
            return await _mediator.Send(new GetAllFinalTransferRequestQuery());
        }

        [HttpPut("approve-finalTransfer")]
        [Authorize(Roles = "GuardQC")]
        public async Task<IActionResult> UpdateApproveAsync([FromQuery] ApproveFinalTransferRequestCommand command)
        {
            await _mediator.Send(command);
            return Ok("Duyệt đơn chuyển giao cuối cùng thành công");
        }
    }
}
