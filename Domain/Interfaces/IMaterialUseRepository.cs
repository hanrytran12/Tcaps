using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialUseRepository
    {
        Task<IEnumerable<MaterialUse>> GetAllAsync();
        Task<MaterialUse> GetByIdAsync(Guid id);
        Task<List<MaterialUse>> GetByAssignIdAsync(Guid assignId);
        void Update(MaterialUse materialUse);
        Task AddAsync(MaterialUse materialUse);
    }
}
