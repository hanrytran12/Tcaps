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
    public class MaterialSupplyRepository : IMaterialSupplyRepository
    {
        private readonly AppDbContext _context;

        public MaterialSupplyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaterialSupply materialSupply)
        {
            await _context.MaterialSupplies.AddAsync(materialSupply);
        }

        public void Delete(MaterialSupply materialSupply)
        {
            _context.MaterialSupplies.Remove(materialSupply);
        }

        public async Task<IEnumerable<MaterialSupply>> GetAllAsync()
        {
            return await _context.MaterialSupplies.ToListAsync();
        }

        public async Task<MaterialSupply> GetByIdAsync(Guid id)
        {
            return await _context.MaterialSupplies.FindAsync(id);
        }

        public async Task<IEnumerable<MaterialSupply>> GetByUserIdAsync(Guid userId)
        {
            return await _context.MaterialSupplies
                .Where(m => m.SupplierId == userId)
                .ToListAsync();
        }

        public void Update(MaterialSupply materialSupply)
        {
            _context.MaterialSupplies.Update(materialSupply);
        }
    }
}
