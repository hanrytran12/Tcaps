using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Query.GetAllProductionByQCId
{
    public class GetAllProductionByQCIdQueryHandler : IRequestHandler<GetAllProductionByQCIdQuery, List<ProductionDTO>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppDbContext _context;

        public GetAllProductionByQCIdQueryHandler(IProductionRepository productionRepository,
            IUserRepository userRepository, IAppDbContext context)
        {
            _productionRepository = productionRepository;
            _userRepository = userRepository;
            _context = context;
        }
        public async Task<List<ProductionDTO>> Handle(GetAllProductionByQCIdQuery request, CancellationToken cancellationToken)
        {
            var query = _productionRepository.Query();

            var workshopId = await _userRepository.GetWorkshopIdByQCIdAsync(request.QC_Id);
            if (workshopId == Guid.Empty)
                throw new InvalidOperationException($"Workshop for QC {request.QC_Id} not found.");

            var users = await _userRepository.GetUsersByWorkshopIdAsync(workshopId);
            if (users == null || !users.Any())
                //throw new InvalidOperationException($"No staff found for workshop.");
                return new List<ProductionDTO>();

            var userIds = users.Select(u => u.Id).ToList();

            query = query.Where(q => userIds.Contains(q.UserId));

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query
                    .Where(q => q.Status == request.Status);
            }

            var productions = await query.ToListAsync(cancellationToken);

            var productionDTOs = new List<ProductionDTO>();

            foreach (var p in productions)
            {
                var assignment = await _context.Assignments.FindAsync(p.AssignId);
                if (assignment == null) continue;

                var batch = await _context.Batches.FindAsync(assignment.BatchId);
                var user = users.FirstOrDefault(u => u.Id == p.UserId);

                productionDTOs.Add(new ProductionDTO
                {
                    Id = p.Id,
                    AssignId = p.AssignId,
                    BatchCode = batch?.Code ?? string.Empty,
                    UserId = p.UserId,
                    FullName = user?.FullName ?? string.Empty,
                    Quantity = p.Quantity,
                    Date = p.Date,
                    Time = p.Time,
                    Status = p.Status
                });
            }

            return productionDTOs
                .OrderByDescending(p => p.Date)
                .ToList();
        }
    }
}
