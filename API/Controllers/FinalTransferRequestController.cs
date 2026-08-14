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
    public class FinalTransferRequestController : BaseApiController
    {
        public FinalTransferRequestController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("all")]
        [Authorize(Roles = "GuardQC")]
        public async Task<ActionResult<List<FinalTransferRequestDTO>>> GetAllAsync()
        {
            var result = await Mediator.Send(new GetAllFinalTransferRequestQuery());
            return Ok(result);
        }

        [HttpPut("approve-finalTransfer")]
        [Authorize(Roles = "GuardQC")]
        public async Task<IActionResult> UpdateApproveAsync([FromBody] ApproveFinalTransferRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Duyệt đơn chuyển giao cuối cùng thành công.");
        }
    }
}
