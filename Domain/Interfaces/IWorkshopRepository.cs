using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkshopRepository
    {
        Task<Workshop?> GetByIdAsync(Guid id);
        Task<IEnumerable<Workshop>> GetAllAsync();
        Task<IEnumerable<Workshop>> FindByNameAsync(string name);
        Task AddAsync(Workshop workshop);
        void Update(Workshop workshop);
        void Delete(Workshop workshop);
    }
}
