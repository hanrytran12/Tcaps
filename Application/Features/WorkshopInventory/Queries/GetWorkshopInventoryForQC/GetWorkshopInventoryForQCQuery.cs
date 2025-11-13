using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryForQC
{
    public class GetWorkshopInventoryForQCQuery : IRequest<Result<List<WorkshopInventoryForQCDTO>>>
    {
        public Guid UserId { get; set; }
    }
}
