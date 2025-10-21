using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEvaluateRepository
    {
        Task<IEnumerable<Evaluate>> GetAllAsync();
        Task<IEnumerable<Evaluate>> GetByProductionIdAsync(Guid productionId);
        Task<IEnumerable<Evaluate>> GetByProductionIdsAsync(List<Guid> productionIds);
        Task<Evaluate> GetByIdAsync(Guid id);
        Task AddAsync(Evaluate evaluate);
        void Update(Evaluate evaluate);
    }
}
