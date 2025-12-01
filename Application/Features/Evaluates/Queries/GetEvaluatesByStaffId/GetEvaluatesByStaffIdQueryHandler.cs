using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQueryHandler : IRequestHandler<GetEvaluatesByStaffIdQuery, Result<List<EvaluateDTO>>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetEvaluatesByStaffIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }
        public async Task<Result<List<EvaluateDTO>>> Handle(GetEvaluatesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var query = from e in _context.Evaluates.AsNoTracking()
                        join p in _context.Productions.AsNoTracking() on e.ProductionId equals p.Id
                        where p.UserId == request.StaffId && p.AssignId == request.AssignId
                        select new
                        {
                            e.Id,
                            e.ProductionId,
                            e.QuantityError,
                            e.QuantitySuccess,
                            e.Note,
                            e.Status,
                            e.CreatedAt,
                            RawImageString = e.Image,

                            Defects = e.ComponentDefects.Select(cd => new ComponentDefectsDTO
                            {
                                Id = cd.Id,
                                Description = cd.Description,
                                Quantity = cd.Quantity,
                                Status = cd.Status,
                            }).ToList()
                        };

            var rawData = await query.ToListAsync(cancellationToken);

            if (!rawData.Any())
            {
                var staffExists = await _context.Users.AsNoTracking()
                    .AnyAsync(u => u.Id == request.StaffId, cancellationToken);

                if (!staffExists)
                {
                    return Result<List<EvaluateDTO>>.Failure($"Không tìm thấy User với ID = {request.StaffId}.");
                }

                return Result<List<EvaluateDTO>>.Success(new List<EvaluateDTO>());
            }

            var resultDtos = rawData.Select(item => new EvaluateDTO
            {
                Id = item.Id,
                ProductionId = item.ProductionId,
                QuantityError = item.QuantityError,
                QuantitySuccess = item.QuantitySuccess,
                Note = item.Note,
                Status = item.Status,
                Created_At = item.CreatedAt,
                Defects = item.Defects,

                Images = string.IsNullOrEmpty(item.RawImageString)
                    ? new List<string>()
                    : item.RawImageString
                          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(path => _fileStorageService.GetFileUrl(path.Trim()))
                          .ToList()
            }).ToList();

            return Result<List<EvaluateDTO>>.Success(resultDtos);
        }
    }
}
