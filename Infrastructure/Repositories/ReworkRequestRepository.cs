using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReworkRequestRepository : IReworkRequestRepository
    {
        private readonly AppDbContext _context;

        public ReworkRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReworkRequestAsync(ReworkRequest reworkRequest)
        {
            await _context.ReworkRequests.AddAsync(reworkRequest);
        }

        public async Task<ReworkRequest?> GetByAssignIdAsync(Guid assignmentId)
        {
            return await _context.ReworkRequests
                .FirstOrDefaultAsync(r => r.AssignmentId == assignmentId);
        }
    }
}
