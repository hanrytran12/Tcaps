using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialSupplies.Query.GetAllMaterialSupplies
{
    public class GetAllMaterialSuppliesQuery : IRequest<Result<List<MaterialSupplyDTO>>>
    {
        public string? Role { get; set; }         // "Lead", "QcTransport", "QC"
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
    }
}
