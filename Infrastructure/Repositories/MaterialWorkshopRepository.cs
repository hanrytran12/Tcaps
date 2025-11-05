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
    public class MaterialWorkshopRepository : IMaterialWorkshopRepository
    {
        private readonly AppDbContext _context;

        public MaterialWorkshopRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaterialWorkshop materialWorkshop)
        {
            await _context.MaterialWorkshops.AddAsync(materialWorkshop);
        }

        public void Delete(MaterialWorkshop materialWorkshop)
        {
            _context.MaterialWorkshops.Remove(materialWorkshop);
        }

        public async Task<IEnumerable<MaterialWorkshop>> GetAllAsync()
        {
            return await _context.MaterialWorkshops.ToListAsync();
        }

        public async Task<IEnumerable<MaterialWorkshop>> GetAllByWorkshopIdAsync(Guid workshopId)
        {
            return await _context.MaterialWorkshops
                .Where(m => m.WorkshopId == workshopId)
                .ToListAsync();
        }

        public async Task<MaterialWorkshop> GetByIdAsync(Guid id)
        {
            return await _context.MaterialWorkshops.FindAsync(id);
        }

        public void Update(MaterialWorkshop materialWorkshop)
        {
            _context.MaterialWorkshops.Update(materialWorkshop);
        }
    }
}
