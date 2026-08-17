using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop
{
    public class GetAllMaterialWorkshopQueryHandler : IRequestHandler<GetAllMaterialWorkshopQuery, List<Application.DTOs.Response.MaterialWorkshopSummaryDTO>>
    {
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;

        public GetAllMaterialWorkshopQueryHandler(IMaterialWorkshopRepository materialWorkshopRepository)
        {
            _materialWorkshopRepository = materialWorkshopRepository;
        }
        public async Task<List<Application.DTOs.Response.MaterialWorkshopSummaryDTO>> Handle(GetAllMaterialWorkshopQuery request, CancellationToken cancellationToken)
        {
            var materialWorkshops = await _materialWorkshopRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                materialWorkshops = materialWorkshops.Where(m => m.Status == request.Status).ToList();
            }

            return materialWorkshops
                .OrderByDescending(materialWorkshop => materialWorkshop.CreatedAt)
                .Select(materialWorkshop => new Application.DTOs.Response.MaterialWorkshopSummaryDTO
                {
                    Id = materialWorkshop.Id,
                    WorkshopId = materialWorkshop.WorkshopId,
                    AssignId = materialWorkshop.AssignId,
                    AssignmentTransferRequestId = materialWorkshop.AssignmentTransferRequestId,
                    SupplierId = materialWorkshop.SupplierId,
                    QuantitySend = materialWorkshop.QuantitySend,
                    QuantityReceive = materialWorkshop.QuantityReceive,
                    ShipDate = materialWorkshop.ShipDate,
                    CreatedAt = materialWorkshop.CreatedAt,
                    Status = materialWorkshop.Status
                })
                .ToList();
        }
    }
}
