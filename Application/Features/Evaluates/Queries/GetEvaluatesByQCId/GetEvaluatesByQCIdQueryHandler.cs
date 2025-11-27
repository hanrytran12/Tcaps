using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQueryHandler : IRequestHandler<GetEvaluatesByQCIdQuery, Result<List<EvaluateDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetEvaluatesByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
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
                    Image = e.Image,
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
