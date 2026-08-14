using Application.DTOs.Response;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Commands.UpdateLeadForBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchById;
using Application.Features.Batches.Queries.GetBatchByWorkshopId;
using Application.Features.Batches.Queries.GetBatchesByQCId;
using Application.Features.Batches.Queries.GetBatchesByStaffId;
using Application.Features.Batches.Queries.GetBatchForLead;
using Application.Features.Batches.Queries.GetBatchForManagement;
using Application.Features.Batches.Queries.GetDashboardStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchController : BaseApiController
    {
        public BatchController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<List<BatchResponseDTO>>> GetAllBatch()
        {
            var result = await Mediator.Send(new GetAllBatchQuery());
            return Ok(result);
        }

        [HttpGet("management")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<ActionResult<List<BatchDTO>>> GetBatchForManagement()
        {
            var result = await Mediator.Send(new GetBatchForManagementQuery());
            return Ok(result);
        }

        [HttpGet("{batchId:guid}")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<BatchDetailResponseDTO>> GetBatchById(Guid batchId)
        {
            var result = await Mediator.Send(new GetBatchByIdQuery(batchId));
            return Ok(result);
        }

        [HttpGet("dashboard")]
        [Authorize(Policy = "CanViewDashboard")]
        public async Task<ActionResult<DashboardResultDTO>> GetDashboardStats([FromQuery] GetDashboardStatsQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<BatchDTO>>> GetBatchByWorkshopId([FromQuery] string? Status, DateOnly? FromDate, DateOnly? ToDate)
        {
            var result = await Mediator.Send(new GetBatchByWorkshopIdQuery(CurrentUserId, Status, FromDate, ToDate));
            return Ok(result);
        }

        [HttpGet("staff/batches")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<StaffSummaryDashboardDTO>>> GetBatchesByStaffIdAsync()
        {
            var result = await Mediator.Send(new GetBatchesByStaffIdQuery
            {
                StaffId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpGet("qc/batches")]
        [Authorize(Roles = "QC,QCK")]
        public async Task<ActionResult<List<BatchForQCDTO>>> GetBatchesByQCIdAsync()
        {
            var result = await Mediator.Send(new GetBatchesByQCIdQuery
            {
                QcId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpGet("lead/batches")]
        [Authorize(Policy = "Lead")]
        public async Task<ActionResult<List<BatchResponseDTO>>> GetBatchesByLeadIdAsync()
        {
            var result = await Mediator.Send(new GetBatchForLeadQuery
            {
                UserId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddBatch([FromBody] AddBatchCommand command)
        {
            await Mediator.Send(command);
            return Ok(new { message = "Tạo lô hàng thành công." });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateBatch(Guid id, [FromBody] UpdateBatchCommand command)
        {
            command.Id = id;
            await Mediator.Send(command);
            return Ok(new { message = "Cập nhật lô hàng thành công." });
        }

        [HttpPut("lead-for-batch")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeadForBatch([FromBody] UpdateLeadForBatchCommand command)
        {
            await Mediator.Send(command);
            return Ok(new { message = "Cập nhật Lead cho lô hàng thành công." });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteBatch(Guid id)
        {
            await Mediator.Send(new DeleteBatchCommand(id));
            return Ok(new { message = "Xóa lô hàng thành công." });
        }
    }
}
