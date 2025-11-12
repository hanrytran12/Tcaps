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
        Task<MaterialSupply> GetByIdAsync(Guid id);
        Task AddAsync(MaterialSupply materialSupply);
        void Update(MaterialSupply materialSupply);
        void Delete(MaterialSupply materialSupply);
    }
}
