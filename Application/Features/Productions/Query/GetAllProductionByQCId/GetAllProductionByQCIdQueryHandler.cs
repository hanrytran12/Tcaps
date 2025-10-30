using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Query.GetAllProductionByQCId
{
    public class GetAllProductionByQCIdQueryHandler : IRequestHandler<GetAllProductionByQCIdQuery, List<ProductionDTO>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllProductionByQCIdQueryHandler(IProductionRepository productionRepository,
            IUserRepository userRepository, IMapper mapper)
        {
            _productionRepository = productionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductionDTO>> Handle(GetAllProductionByQCIdQuery request, CancellationToken cancellationToken)
        {
            var query = _productionRepository.Query();

            var workshopId = await _userRepository.GetWorkshopIdByQCIdAsync(request.QC_Id);
            if (workshopId == Guid.Empty)
                throw new InvalidOperationException($"Workshop for QC {request.QC_Id} not found.");

            var users = await _userRepository.GetUsersByWorkshopIdAsync(workshopId);
            if (users == null || !users.Any())
                throw new InvalidOperationException($"No staff found for workshop.");

            var userIds = users.Select(u => u.Id).ToList();

            query = query.Where(q => userIds.Contains(q.UserId));

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query
                    .Where(q => q.Status == request.Status);
            }

            var productions = await query.ToListAsync(cancellationToken);
            var dto = _mapper.Map<List<ProductionDTO>>(productions);

            return dto;
        }
    }
}
