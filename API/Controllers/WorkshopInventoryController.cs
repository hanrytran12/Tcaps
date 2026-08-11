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
        public async Task<List<Domain.Entities.WorkshopInventory>> GetAllWorkshopInventory()
        {
            return await Mediator.Send(new GetAllWorkshopInventoryQuery());
        }

        [HttpGet("{workshopId:guid}")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport")]
        public async Task<List<WorkshopInventoryForExportDTO>> GetWorkshopInvenntoryByWorkshopId(Guid workshopId)
        {
            return await Mediator.Send(new GetWorkshopInventoryByWorkshopIdQuery(workshopId));
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<WorkshopInventoryForQCDTO>> GetWorkshopInventoryForQC()
        {
            return await Mediator.Send(new GetWorkshopInventoryForQCQuery());
        }

        [HttpGet("by-material")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<Domain.Entities.WorkshopInventory> GetByMaterialId([FromQuery] GetWorkshopInventoryByMaterialIdQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
