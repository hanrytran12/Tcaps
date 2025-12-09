using Application.DTOs.Response;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchById;
using Application.Features.Batches.Queries.GetBatchByWorkshopId;
using Application.Features.Batches.Queries.GetBatchesByQCId;
using Application.Features.Batches.Queries.GetBatchesByStaffId;
using Application.Features.Batches.Queries.GetBatchForManagement;
using Application.Features.Batches.Queries.GetDashboardStats;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchController : BaseApiController
    {
        [HttpGet]
        public async Task<List<Batch>> GetAllBatch()
        {
            return await Mediator.Send(new GetAllBatchQuery());
        }

        [HttpGet("management")]
        public async Task<List<BatchDTO>> GetBatchForManagement()
        {
            return await Mediator.Send(new GetBatchForManagementQuery());
        }

        [HttpGet("{batchId:guid}")]
        public async Task<BatchDetailResponseDTO> GetBatchById(Guid batchId)
        {
            return await Mediator.Send(new GetBatchByIdQuery(batchId));
        }

        [HttpGet("dashboard")]
        [Authorize(Policy = "CanViewDashboard")]
        public async Task<DashboardResultDTO> GetDashboardStats([FromQuery] GetDashboardStatsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<BatchDTO>> GetBatchByWorkshopId([FromQuery] string? Status, DateOnly? FromDate, DateOnly? ToDate)
        {
            return await Mediator.Send(new GetBatchByWorkshopIdQuery(CurrentUserId, Status, FromDate, ToDate));
        }

        [HttpGet("staff/bactches")]
        public async Task<List<BatchDTO>> GetBatchesByStaffIdAsync()
        {
            return await Mediator.Send(new GetBatchesByStaffIdQuery
            {
                StaffId = CurrentUserId
            });
        }

        [HttpGet("qc/bactches")]
        [Authorize(Policy = "QC")]
        public async Task<List<BatchDTO>> GetBatchesByQCIdAsync()
        {
            return await Mediator.Send(new GetBatchesByQCIdQuery
            {
                QcId = CurrentUserId
            });
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddBatch(AddBatchCommand command)
        {
            await Mediator.Send(command);
            return Ok("Create batch successfully");
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateBatch(Guid id, [FromBody] UpdateBatchCommand command)
        {
            command.Id = id;
            await Mediator.Send(command);
            return Ok("Update batch successfully");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteBatch(Guid id)
        {
            await Mediator.Send(new DeleteBatchCommand(id));
            return Ok("Delete batch successfully");
        }
    }
}
