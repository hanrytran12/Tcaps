using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Productions.Query.GetAllProductionByStaffId
{
    public class GetAllProductionByStaffIdQuery : IRequest<List<ProductionDTO>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string? Status { get; set; }
    }
}
