using Application.DTOs.Response;
using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Command.NotifyQCMaterialShortage;
using Application.Features.Productions.Command.UpdateProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByAssignId;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/production")]
    [ApiController]
    public class ProductionController : BaseApiController
    {
        [HttpGet("all")]
        public async Task<List<ProductionDTO>> GetAllAsync()
        {
            return await Mediator.Send(new GetAllProductionQuery());
        }

        [HttpGet("for-staff")]
        public async Task<List<ProductionDTO>> GetByStaffIdAsync([FromQuery] string? status)
        {
            return await Mediator.Send(new GetAllProductionByStaffIdQuery
            {
                UserId = CurrentUserId,
                Status = status
            });
        }

        [HttpGet("for-qc")]
        public async Task<List<ProductionDTO>> GetProductionsWithStatusPendingQC([FromQuery] string? status)
        {
            return await Mediator.Send(new GetAllProductionByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            });
        }

        [HttpGet("by-assignId")]
        public async Task<List<ProductionDTO>> GetProductionByAssignIdAsync([FromQuery] Guid assignId)
        {
            return await Mediator.Send(new GetAllProductionByAssignIdQuery
            {
                AssignId = assignId,
                UserId = CurrentUserId,
            });
        }

        [HttpPost("report-work")]
        public async Task<IActionResult> ReportWork([FromBody] AddProductionReportCommand command)
        {
            command.StaffId = CurrentUserId;
            var result = await Mediator.Send(command);
            if (result.IsFailure)
                return BadRequest(result.error);
            return Ok("Nộp sản phẩm thành công.");
        }

        [HttpPost("notify-material-shortage")]
        public async Task<IActionResult> NotifyMaterialShortage([FromBody] NotifyQCMaterialShortageCommand command)
        {
            command.StaffId = CurrentUserId;
            var result = await Mediator.Send(command);
            if (result.IsFailure)
                return BadRequest(result.error);
            return Ok("Đã gửi thông báo hết NVL đến QC thành công.");
        }

        [HttpPut("for-qc/reduce-quantity")]
        [Authorize(Roles = "QC")]
        public async Task<IActionResult> UpdateQuantity([FromQuery] UpdateProductionCommand command)
        {
            await Mediator.Send(command);
            return Ok("Cập nhật số lượng thành công.");
        }
    }
}
