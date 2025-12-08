using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQueryHandler : IRequestHandler<GetEvaluatesByQCIdQuery, List<EvaluateDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetEvaluatesByQCIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<EvaluateDTO>> Handle(GetEvaluatesByQCIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Evaluates
                .AsNoTracking()
                .Where(e => e.UserId == request.QC_Id);

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(e => e.Status == request.Status);
            }

            var rawData = await query
                .Select(e => new
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
                })
                .ToListAsync(cancellationToken);

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
            return resultDtos;
        }
    }
}
