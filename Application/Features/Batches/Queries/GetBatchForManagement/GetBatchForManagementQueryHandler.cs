using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchForManagement
{
    public class GetBatchForManagementQueryHandler : IRequestHandler<GetBatchForManagementQuery, List<BatchDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetBatchForManagementQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<BatchDTO>> Handle(GetBatchForManagementQuery request, CancellationToken cancellationToken)
        {
            var batchList = from b in _appDbContext.Batches
                            join p in _appDbContext.Products on b.ProductId equals p.Id
                            select new BatchDTO
                            {
                                BatchId = b.Id,
                                Code = b.Code,
                                ProductName = p.Name,
                                Quantity = b.Quantity,
                                StartDate = b.StartDate,
                                EndDate = b.EndDate,
                                Status = b.Status,
                            };

            return await batchList.ToListAsync();
        }
    }
}
