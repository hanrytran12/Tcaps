using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssignmentTransferRequestRepository : IAssignmentTransferRequestRepository
    {
        private readonly AppDbContext _appDbContext;

        public AssignmentTransferRequestRepository(AppDbContext appDbContext)
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

        public void Update(AssignmentTransferRequest assignmentTransferRequest)
        {
            _appDbContext.AssignmentTransferRequests.Update(assignmentTransferRequest);
        }
    }
}
