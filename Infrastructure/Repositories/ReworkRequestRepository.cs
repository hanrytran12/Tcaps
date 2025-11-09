using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

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
    }
}
