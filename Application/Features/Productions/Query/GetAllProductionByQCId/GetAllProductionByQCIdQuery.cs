using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Productions.Query.GetAllProductionByQCId
{
    public class GetAllProductionByQCIdQuery : IRequest<List<ProductionDTO>>
    {
        public Guid QC_Id { get; set; }
        public string? Status { get; set; }
    }
}
