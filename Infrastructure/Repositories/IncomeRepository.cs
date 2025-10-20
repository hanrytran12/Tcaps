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
    public class IncomeRepository : IIncomeRepository
    {
        private readonly AppDbContext _context;

        public IncomeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Income income)
        {
            await _context.Incomes.AddAsync(income);
        }

        public async Task<IEnumerable<Income>> GetAllAsync()
        {
            return await _context.Incomes.ToListAsync(); 
        }

        public async Task<Income> GetByIdAsync(Guid id)
        {
            return await _context.Incomes.FindAsync(id);
        }

        public async Task<IEnumerable<Income>> GetIncomeHistoryAsync(Guid userId)
        {
            return await _context.Incomes
                .Where(i => i.UserId == userId).ToListAsync();
        }

        public async Task<decimal> GetTotalIncomeAsync(Guid userId)
        {
            return await _context.Incomes
                .Where(i => i.UserId == userId)
                .Select(i => i.TotalPrice)
                .DefaultIfEmpty(0)
                .SumAsync();
        }

        public void Update(Income income)
        {
            _context.Incomes.Update(income);
        }
    }
}
