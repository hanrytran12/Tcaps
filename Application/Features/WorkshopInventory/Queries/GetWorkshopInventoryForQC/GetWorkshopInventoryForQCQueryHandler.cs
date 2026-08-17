using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryForQC
{
    public class GetWorkshopInventoryForQCQueryHandler : IRequestHandler<GetWorkshopInventoryForQCQuery, List<WorkshopInventoryForQCDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetWorkshopInventoryForQCQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<WorkshopInventoryForQCDTO>> Handle(GetWorkshopInventoryForQCQuery request, CancellationToken cancellationToken)
        {
            var user = await _appDbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == request.UserId)
                .Select(u => new { u.Id, u.WorkshopId })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var query = from w in _appDbContext.WorkshopInventories
                        join m in _appDbContext.Materials on w.MaterialId equals m.Id
                        join a in _appDbContext.Assignments on w.WorkshopId equals a.WorkshopId
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        where w.WorkshopId == user.WorkshopId
                        select new WorkshopInventoryForQCDTO
                        {
                            WorkshopInventoryId = w.Id,
                            WorkshopId = w.WorkshopId,
                            AssignId = a.Id,
                            MaterialId = w.MaterialId,
                            MaterialName = m.Name,
                            BatchCode = b.Code,
                            Quantity = (int)w.Quantity,
                        };

            var result = await query
                .Distinct()
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}
