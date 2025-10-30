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
    public class ComponentDefectRepository : IComponentDefectRepository
    {
        private readonly AppDbContext _context;

        public ComponentDefectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ComponentDefect componentDefect)
        {
            await _context.ComponentDefects.AddAsync(componentDefect);
        }

        public void Delete(ComponentDefect componentDefect)
        {
            _context.ComponentDefects.Remove(componentDefect);
        }

        public async Task<IEnumerable<ComponentDefect>> GetAllAsync()
        {
            return await _context.ComponentDefects.ToListAsync();
        }

        public async Task<IEnumerable<ComponentDefect>> GetAllByEvaluateIdAsync(Guid evaluateId)
        {
            return await _context.ComponentDefects
                .Where(cd => cd.EvaluateId == evaluateId)
                .ToListAsync();
        }

        public async Task<ComponentDefect?> GetByIdAsync(Guid id)
        {
            return await _context.ComponentDefects.FindAsync(id);
        }

        public void Update(ComponentDefect componentDefect)
        {
            _context.ComponentDefects.Update(componentDefect);
        }
    }
}
