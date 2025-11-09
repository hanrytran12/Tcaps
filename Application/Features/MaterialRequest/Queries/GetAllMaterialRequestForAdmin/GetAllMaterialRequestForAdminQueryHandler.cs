using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequestForAdmin
{
    public class GetAllMaterialRequestForAdminQueryHandler : IRequestHandler<GetAllMaterialRequestForAdminQuery, Result<List<Domain.Entities.MaterialRequest>>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public GetAllMaterialRequestForAdminQueryHandler(IMaterialRequestRepository materialRequestRepository)
        {
            _materialRequestRepository = materialRequestRepository;
        }
        public async Task<Result<List<Domain.Entities.MaterialRequest>>> Handle(GetAllMaterialRequestForAdminQuery request, CancellationToken cancellationToken)
        {
            var materialRequests = await _materialRequestRepository.GetAllAsync();
            if (materialRequests == null || !materialRequests.Any())
            {
                return Result<List<Domain.Entities.MaterialRequest>>.Failure("Không có yêu cầu thêm vật liệu.");
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                materialRequests = materialRequests
                    .Where(m => string.Equals(m.Status, request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Result<List<Domain.Entities.MaterialRequest>>.Success(materialRequests.ToList());
        }
    }
}
