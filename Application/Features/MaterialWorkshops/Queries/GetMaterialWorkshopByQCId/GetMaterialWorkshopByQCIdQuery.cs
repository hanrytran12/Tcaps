using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId
{
    public class GetMaterialWorkshopByQCIdQuery : IRequest<List<MaterialWorkshop>>
    {
        public Guid QC_Id { get; set; }
        public Guid WorkshopId { get; set; }
        public string? Status { get; set; }
    }
}
