using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductionRepository
    {
        Task<IEnumerable<Production>> GetAllAsync();
        Task<Production?> GetByIdAsync(Guid id);
        Task<User?> GetStaffByProductionIdAsync(Guid productionId);
        Task<IEnumerable<Production>> GetByUserAsync(Guid userId);
        Task AddAsync(Production production);
        void Update(Production production);
        IQueryable<Production> Query();
    }
}
