using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Query.GetAllProductionByStaffId
{
    public class GetAllProductionByStaffIdQueryHandler : IRequestHandler<GetAllProductionByStaffIdQuery, List<ProductionDTO>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetAllProductionByStaffIdQueryHandler(IProductionRepository productionRepository, IMapper mapper,
            IUserRepository userRepository)
        {
            _productionRepository = productionRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<ProductionDTO>> Handle(GetAllProductionByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var query = _productionRepository.Query();
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User không tồn tại");
            }
            query = query.Where(q => q.UserId == request.UserId);

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(q => q.Status == request.Status);
            }

            var productions = await query.ToListAsync(cancellationToken);
            //var dtos = _mapper.Map<List<ProductionDTO>>(productions);
            var dtos = productions.Select(p =>
            {
                return new ProductionDTO
                {
                    Id = p.Id,
                    AssignId = p.AssignId,
                    UserId = p.UserId,
                    FullName = user.FullName,
                    Quantity = p.Quantity,
                    Date = p.Date,
                    Status = p.Status
                };
            })
            .OrderByDescending(p => p.Date)
            .ToList();
            return dtos;
        }
    }
}
