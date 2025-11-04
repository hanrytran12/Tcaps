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
        Task<IEnumerable<Evaluate>> GetByProductionIdsAsync(Guid productionId);
        Task<IEnumerable<Evaluate>> GetByQCIdAsync(Guid qcId);
        Task<IEnumerable<Evaluate>> GetByProductionIdsAsync(List<Guid> productionIds);
        Task<Evaluate> GetByIdAsync(Guid id);
        Task<Evaluate> GetEvaluateByProductionIdAsync(Guid productionId);
        Task AddAsync(Evaluate evaluate);
        void Update(Evaluate evaluate);
    }
}
