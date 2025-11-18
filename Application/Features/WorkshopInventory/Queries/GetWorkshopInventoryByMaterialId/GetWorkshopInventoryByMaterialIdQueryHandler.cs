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

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQueryHandler : IRequestHandler<GetWorkshopInventoryByMaterialIdQuery, Result<Domain.Entities.WorkshopInventory>>
    {
        private readonly IWorkshopInventoryRepository _repository;
        private readonly IMapper _mapper;

        public GetWorkshopInventoryByMaterialIdQueryHandler(IWorkshopInventoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<Domain.Entities.WorkshopInventory>> Handle(GetWorkshopInventoryByMaterialIdQuery request, CancellationToken cancellationToken)
        {
            var workshopInventory = await _repository.GetByMaterialIdAsync(request.WorkshopInventoryId);
            if (workshopInventory == null)
            {
                return Result<Domain.Entities.WorkshopInventory>.Failure("Workshop inventory not found.");
            }

            return Result<Domain.Entities.WorkshopInventory>.Success(workshopInventory);
        }
    }
}
