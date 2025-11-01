using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

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
    }
}
