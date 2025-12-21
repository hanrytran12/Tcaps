using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialSupplyRepository
    {
        Task<IEnumerable<MaterialSupply>> GetAllAsync();
        Task<IEnumerable<MaterialSupply>> GetByUserIdAsync(Guid userId);
        Task<MaterialSupply> GetByIdAsync(Guid id);
        Task AddAsync(MaterialSupply materialSupply);
        void Update(MaterialSupply materialSupply);
        void Delete(MaterialSupply materialSupply);
    }
}
