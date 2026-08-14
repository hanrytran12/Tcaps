using Application.DTOs.Response;
using Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory;
using Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId;
using Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId;
using Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryForQC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopInventoryController : BaseApiController
    {
        public WorkshopInventoryController(ISender mediator) : base(mediator)
        {
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK")]
        public async Task<ActionResult<List<WorkshopInventoryDTO>>> GetAllWorkshopInventory()
        {
            var result = await Mediator.Send(new GetAllWorkshopInventoryQuery());
            return Ok(result);
        }

        [HttpGet("{workshopId:guid}")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport")]
        public async Task<ActionResult<List<WorkshopInventoryForExportDTO>>> GetWorkshopInventoryByWorkshopId(Guid workshopId)
        {
            var result = await Mediator.Send(new GetWorkshopInventoryByWorkshopIdQuery(workshopId));
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<WorkshopInventoryForQCDTO>>> GetWorkshopInventoryForQC()
        {
            var result = await Mediator.Send(new GetWorkshopInventoryForQCQuery());
            return Ok(result);
        }

        [HttpGet("by-material")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<WorkshopInventoryDTO>> GetByMaterialId([FromQuery] GetWorkshopInventoryByMaterialIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
