using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchByWorkshopId
{
    public class GetBatchByWorkshopIdQueryHandler : IRequestHandler<GetBatchByWorkshopIdQuery, List<BatchDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly IUserRepository _userRepository;

        public GetBatchByWorkshopIdQueryHandler(IAppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<List<BatchDTO>> Handle(GetBatchByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            var query = from assignment in _context.Assignments
                        where assignment.WorkshopId == user.WorkshopId
                        join batch in _context.Batches on assignment.BatchId equals batch.Id
                        join product in _context.Products on batch.ProductId equals product.Id
                        join u in _context.Users on batch.UserId equals u.Id
                        select new { batch, product, u };

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(batch => batch.batch.Status == request.Status);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(x => x.batch.StartDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(x => x.batch.EndDate <= request.ToDate.Value);
            }

            var finalQuery = query.Select(batch => new BatchDTO
            {
                ProductName = batch.product.Name,
                UserId = batch.batch.UserId,
                LeadName = batch.u.FullName,
                Code = batch.batch.Code,
                Quantity = batch.batch.Quantity,
                StartDate = batch.batch.StartDate,
                EndDate = batch.batch.EndDate,
                Status = batch.batch.Status,
            });

            return await finalQuery.AsNoTracking().ToListAsync();
        }
    }
}
