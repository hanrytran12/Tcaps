using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialWorkshopRepository
    {
        Task<IEnumerable<MaterialWorkshop>> GetAllAsync();
        Task<MaterialWorkshop> GetByIdAsync(Guid id);
        Task AddAsync(MaterialWorkshop materialWorkshop);
        void Update(MaterialWorkshop materialWorkshop);
        void Delete(MaterialWorkshop materialWorkshop);
    }
}
