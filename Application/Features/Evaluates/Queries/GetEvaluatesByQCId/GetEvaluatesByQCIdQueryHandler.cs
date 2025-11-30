using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQueryHandler : IRequestHandler<GetEvaluatesByQCIdQuery, Result<List<EvaluateDTO>>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetEvaluatesByQCIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<List<EvaluateDTO>>> Handle(GetEvaluatesByQCIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Evaluates
                .AsNoTracking()
                .Where(e => e.UserId == request.QC_Id);

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(e => e.Status.ToLower() == request.Status.ToLower());
            }

            var resultDtos = await query
                .Select(e => new EvaluateDTO
                {
                    Id = e.Id,
                    ProductionId = e.ProductionId,
                    QuantityError = e.QuantityError,
                    QuantitySuccess = e.QuantitySuccess,
                    Note = e.Note,
                    Image = _fileStorageService.GetFileUrl(e.Image),
                    Status = e.Status,
                    Created_At = e.CreatedAt,

                    Defects = e.ComponentDefects
                        .Select(cd => new ComponentDefectsDTO
                        {
                            Id = cd.Id,
                            Description = cd.Description,
                            Quantity = cd.Quantity,
                            Status = cd.Status,
                        }).ToList()
                })
                .ToListAsync(cancellationToken);
            return Result<List<EvaluateDTO>>.Success(resultDtos);
        }
    }
}
