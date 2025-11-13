using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
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
        private readonly IAppDbContext _context;

        public GetAllProductionByStaffIdQueryHandler(IProductionRepository productionRepository, IMapper mapper,
            IUserRepository userRepository, IAppDbContext context)
        {
            _productionRepository = productionRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _context = context;
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
            var productionDTOs = new List<ProductionDTO>();

            foreach (var p in productions)
            {
                var assignment = await _context.Assignments.FindAsync(p.AssignId);
                if (assignment == null) continue;

                var batch = await _context.Batches.FindAsync(assignment.BatchId);

                productionDTOs.Add(new ProductionDTO
                {
                    Id = p.Id,
                    AssignId = p.AssignId,
                    BatchCode = batch?.Code ?? string.Empty,
                    UserId = p.UserId,
                    FullName = user?.FullName ?? string.Empty,
                    Quantity = p.Quantity,
                    Date = p.Date,
                    Status = p.Status
                });
            }

            return productionDTOs
                .OrderByDescending(p => p.Date)
                .ToList();
        }
    }
}
