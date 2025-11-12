using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialUses.Query.GetMaterialUseByAssignId
{
    public class GetMaterialUseByAssignIdQuery : IRequest<Result<List<MaterialUseDTO>>>
    {
        public Guid AssignId { get; set; }
        public string? MaterialName { get; set; }
    }
}
