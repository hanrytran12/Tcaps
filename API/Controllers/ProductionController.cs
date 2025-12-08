using Application.DTOs.Response;
using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Command.UpdateProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var query = new GetAllProductionByStaffIdQuery
            {
                UserId = CurrentUserId,
                Status = status
            };
            return await Mediator.Send(query);
        }

        [HttpGet("for-qc")]
        public async Task<List<ProductionDTO>> GetProductionsWithStatusPendingQC([FromQuery] string? status)
        {
            var query = new GetAllProductionByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            };
            return await Mediator.Send(query);
        }

        [HttpPost("report-work")]
        public async Task<IActionResult> ReportWork([FromBody] AddProductionReportCommand command)
        {
            command.StaffId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Nộp sản phẩm thành công.");
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
