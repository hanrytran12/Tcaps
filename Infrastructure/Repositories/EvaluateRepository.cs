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
    public class EvaluateRepository : IEvaluateRepository
    {
        private readonly AppDbContext _context;

        public EvaluateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Evaluate evaluate)
        {
            await _context.Evaluates.AddAsync(evaluate);
        }

        public async Task<IEnumerable<Evaluate>> GetAllAsync()
        {
            return await _context.Evaluates.ToListAsync();
        }

        public async Task<Evaluate> GetByIdAsync(Guid id)
        {
            return await _context.Evaluates.FindAsync(id);
        }

        public async Task<IEnumerable<Evaluate>> GetByProductionIdAsync(Guid productionId)
        {
            return await _context.Evaluates.Where(e => e.ProductionId == productionId).ToListAsync();
        }

        public async Task<IEnumerable<Evaluate>> GetByProductionIdsAsync(List<Guid> productionIds)
        {
            return await _context.Evaluates.Where(e => productionIds.Contains(e.ProductionId)).ToListAsync();
        }

        public void Update(Evaluate evaluate)
        {
            _context.Evaluates.Update(evaluate);
        }
    }
}
