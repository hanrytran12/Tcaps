using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop
{
    public class GetAllMaterialWorkshopQuery : IRequest<List<MaterialWorkshop>>
    {
        public string? Status { get; set; }
    }
}
