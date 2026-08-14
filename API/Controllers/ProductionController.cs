using Application.DTOs.Response;
using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Command.NotifyQCMaterialShortage;
using Application.Features.Productions.Command.UpdateProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByAssignId;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/production")]
    [ApiController]
    public class ProductionController : BaseApiController
    {
        public ProductionController(ISender mediator) : base(mediator)
        {
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Lead,QC,QCK")]
        public async Task<ActionResult<List<ProductionDTO>>> GetAllAsync()
        {
            var result = await Mediator.Send(new GetAllProductionQuery());
            return Ok(result);
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<ProductionDTO>>> GetByStaffIdAsync([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetAllProductionByStaffIdQuery
            {
                UserId = CurrentUserId,
                Status = status
            });
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<ProductionDTO>>> GetProductionsWithStatusPendingQC([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetAllProductionByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            });
            return Ok(result);
        }

        [HttpGet("by-assignId")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<ProductionDTO>>> GetProductionByAssignIdAsync([FromQuery] Guid assignId)
        {
            var result = await Mediator.Send(new GetAllProductionByAssignIdQuery
            {
                AssignId = assignId,
                UserId = CurrentUserId,
            });
            return Ok(result);
        }

        [HttpPost("report-work")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ReportWork([FromBody] AddProductionReportCommand command)
        {
            command.StaffId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("notify-material-shortage")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> NotifyMaterialShortage([FromBody] NotifyQCMaterialShortageCommand command)
        {
            command.StaffId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("for-qc/reduce-quantity")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateProductionCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Cập nhật số lượng thành công.");
        }
    }
}
