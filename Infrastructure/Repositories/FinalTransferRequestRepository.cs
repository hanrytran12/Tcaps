using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FinalTransferRequestRepository : IFinalTransferRequestRepository
    {
        private readonly AppDbContext _context;

        public FinalTransferRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FinalTransferRequest request)
        {
            await _context.FinalTransferRequests.AddAsync(request);
        }

        public async Task<IEnumerable<FinalTransferRequest>> GetAllAsync()
        {
            return await _context.FinalTransferRequests.ToListAsync();
        }

        public Task<FinalTransferRequest> GetByIdAsync(Guid id)
        {
            return _context.FinalTransferRequests
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public void UpdateAsync(FinalTransferRequest finalTransferRequest)
        {
            _context.FinalTransferRequests.Update(finalTransferRequest);
        }
    }
}
