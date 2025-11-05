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
    public class ProductionRepository : IProductionRepository
    {
        private readonly AppDbContext _context;

        public ProductionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Production production)
        {
            await _context.Productions.AddAsync(production);
        }

        public async Task<IEnumerable<Production>> GetAllAsync()
        {
            return await _context.Productions.ToListAsync();
        }

        public async Task<IEnumerable<Guid>> GetAllStaffIdByAssignIdAsync(Guid assignId)
        {
            return await _context.Productions
                .Where(p => p.AssignId == assignId)
                .Select(p => p.UserId)
                .ToListAsync();
        }

        public async Task<Production?> GetByIdAsync(Guid id)
        {
            return await _context.Productions.FindAsync(id);
        }

        public async Task<IEnumerable<Production>> GetByUserAsync(Guid userId)
        {
            return await _context.Productions.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<User?> GetStaffByProductionIdAsync(Guid productionId)
        {
            var production = await _context.Productions.FindAsync(productionId);
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == production.UserId);
        }

        public IQueryable<Production> Query()
        {
            return _context.Productions.AsQueryable();
        }

        public async Task<int> TotalProductionByAssignIdAsync(Guid assignId)
        {
            return await _context.Productions
                .Where(p => p.AssignId == assignId)
                .SumAsync(p => p.Quantity);
        }

        public void Update(Production production)
        {
            _context.Productions.Update(production);
        }
    }
}
