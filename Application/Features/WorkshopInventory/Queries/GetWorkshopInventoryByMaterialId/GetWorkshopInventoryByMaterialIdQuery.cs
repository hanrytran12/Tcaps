using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQuery : IRequest<Result<Domain.Entities.WorkshopInventory>>
    {
        public Guid MaterialId { get; set; }
    }
}
