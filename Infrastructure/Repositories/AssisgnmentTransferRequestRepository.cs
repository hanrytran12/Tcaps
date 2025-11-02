using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssisgnmentTransferRequestRepository : IAssignmentTransferRequestRepository
    {
        private readonly AppDbContext _appDbContext;

        public AssisgnmentTransferRequestRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(AssignmentTransferRequest assignmentTransferRequest)
        {
            await _appDbContext.AssignmentTransferRequests.AddAsync(assignmentTransferRequest);
        }

        public async Task<AssignmentTransferRequest?> GetByIdAsync(Guid transferRequestId)
        {
            return await _appDbContext.AssignmentTransferRequests.FirstOrDefaultAsync(x => x.Id == transferRequestId);
        }
    }
}
