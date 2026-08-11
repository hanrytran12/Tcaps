using System;
using System.Collections.Generic;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop
{
    public class GetAllMaterialWorkshopQuery : IRequest<List<Application.DTOs.Response.MaterialWorkshopSummaryDTO>>
    {
        public string? Status { get; set; }
    }
}
