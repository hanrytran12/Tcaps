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
    public class TaskTransferRequestRepository : ITaskTransferRequestRepository
    {
        private readonly AppDbContext _context;

        public TaskTransferRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskTransferRequest request)
        {
            await _context.TaskTransferRequests.AddAsync(request);
        }

        public async Task<IEnumerable<TaskTransferRequest>> GetAllAsync()
        {
            return await _context.TaskTransferRequests.ToListAsync();
        }

        public async Task<TaskTransferRequest?> GetByIdAsync(Guid id)
        {
            return await _context.TaskTransferRequests.FindAsync(id);
        }

        public async Task<IEnumerable<TaskTransferRequest>> GetByQcTransportIdAsync(Guid qcTransportId)
        {
            return await _context.TaskTransferRequests
                .Where(t => t.QcTransportId == qcTransportId)
                .ToListAsync();
        }

        public void Update(TaskTransferRequest request)
        {
            _context.TaskTransferRequests.Update(request);
        }
    }
}
