using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Productions.Query.GetAllProductionByAssignId
{
    public class GetAllProductionByAssignIdQuery : IRequest<List<ProductionDTO>>
    {
        public Guid AssignId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
