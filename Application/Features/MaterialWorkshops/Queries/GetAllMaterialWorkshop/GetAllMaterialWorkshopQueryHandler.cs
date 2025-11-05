using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop
{
    public class GetAllMaterialWorkshopQueryHandler : IRequestHandler<GetAllMaterialWorkshopQuery, List<MaterialWorkshop>>
    {
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;

        public GetAllMaterialWorkshopQueryHandler(IMaterialWorkshopRepository materialWorkshopRepository)
        {
            _materialWorkshopRepository = materialWorkshopRepository;
        }
        public async Task<List<MaterialWorkshop>> Handle(GetAllMaterialWorkshopQuery request, CancellationToken cancellationToken)
        {
            var materialWorkshops = await _materialWorkshopRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                materialWorkshops = materialWorkshops.Where(m => m.Status == request.Status).ToList();
            }

            return materialWorkshops.ToList();
        }
    }
}
