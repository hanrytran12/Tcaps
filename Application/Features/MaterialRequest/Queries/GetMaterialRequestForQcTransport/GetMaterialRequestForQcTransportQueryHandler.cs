using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQcTransport
{
    public class GetMaterialRequestForQcTransportQueryHandler : IRequestHandler<GetMaterialRequestForQcTransportQuery, Result<Domain.Entities.MaterialRequest>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public GetMaterialRequestForQcTransportQueryHandler(IMaterialRequestRepository materialRequestRepository)
        {
            _materialRequestRepository = materialRequestRepository;
        }
        public async Task<Result<Domain.Entities.MaterialRequest>> Handle(GetMaterialRequestForQcTransportQuery request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.MaterialRequestId);
            if (materialRequest == null)
            {
                return Result<Domain.Entities.MaterialRequest>.Failure("Material request not found.");
            }

            return Result<Domain.Entities.MaterialRequest>.Success(materialRequest);
        }
    }
}
