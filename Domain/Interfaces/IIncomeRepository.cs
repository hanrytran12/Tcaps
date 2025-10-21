using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IIncomeRepository
    {
        Task<IEnumerable<Income>> GetAllAsync();
        Task<Income> GetByIdAsync(Guid id);
        Task<decimal> GetTotalIncomeAsync(Guid userId);
        Task<IEnumerable<Income>> GetIncomeHistoryAsync(Guid userId);
        Task AddAsync(Income income);
        void Update(Income income);
    }
}
