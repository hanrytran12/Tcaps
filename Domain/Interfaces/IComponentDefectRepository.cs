using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IComponentDefectRepository
    {
        Task<IEnumerable<ComponentDefect>> GetAllAsync();
        Task<IEnumerable<ComponentDefect>> GetAllByEvaluateIdAsync(Guid evaluateId);
        Task<ComponentDefect?> GetByIdAsync(Guid id);
        Task AddAsync(ComponentDefect componentDefect);
        void Delete(ComponentDefect componentDefect);
        void Update(ComponentDefect componentDefect);
    }
}
