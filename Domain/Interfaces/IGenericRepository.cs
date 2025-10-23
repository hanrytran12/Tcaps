using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T>where T : class
    {
        Task<PageResult<T>> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null, //điều kiện lọc
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, //sắp xếp
        int pageNumber = 1,
        int pageSize = 10);
    }
}
