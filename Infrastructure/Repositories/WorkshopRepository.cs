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
    public class WorkshopRepository : IWorkshopRepository
    {
        private readonly AppDbContext _context;

        public WorkshopRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Workshop workshop)
        {
            await _context.Workshop.AddAsync(workshop);
        }

        public void Delete(Workshop workshop)
        {
           _context.Workshop.Remove(workshop);
        }

        public async Task<IEnumerable<Workshop>> FindByNameAsync(string name)
        {
            return await _context.Workshop.Where(ws => ws.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Workshop>> GetAllAsync()
        {
            return await _context.Workshop.ToListAsync();
        }

        public async Task<Workshop?> GetByIdAsync(Guid id)
        {
            return await _context.Workshop.FindAsync(id);
        }

        public void Update(Workshop workshop)
        {
            _context.Workshop.Update(workshop);
        }
    }
}
