using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequestForAdmin
{
    public class GetAllMaterialRequestForAdminQuery : IRequest<Result<List<MaterialRequestDTO>>>
    {
        public string? Status { get; set; }
    }
}
