using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequestForAdmin;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQC
{
    public class GetMaterialRequestForQCQueryHandler : IRequestHandler<GetMaterialRequestForQCQuery, Result<List<MaterialRequestDTO>>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IAppDbContext _context;

        public GetMaterialRequestForQCQueryHandler(IMaterialRequestRepository materialRequestRepository, IAppDbContext context)
        {
            _materialRequestRepository = materialRequestRepository;
            _context = context;
        }
        public async Task<Result<List<MaterialRequestDTO>>> Handle(GetMaterialRequestForQCQuery request, CancellationToken cancellationToken)
        {
            var materialRequestsQuery = from mr in _context.MaterialRequests.AsNoTracking()

                                        join m in _context.Materials.AsNoTracking()
                                            on mr.MaterialId equals m.Id

                                        join u in _context.Users.AsNoTracking()
                                            on mr.UserId equals u.Id

                                        join w in _context.Workshops.AsNoTracking()
                                            on u.WorkshopId equals w.Id into workshopGroup
                                        from wItem in workshopGroup.DefaultIfEmpty()

                                        where mr.UserId == request.QcId
                                        select new MaterialRequestDTO
                                        {
                                            Id = mr.Id,
                                            MaterialId = mr.MaterialId,
                                            MaterialName = m.Name,
                                            UserId = mr.UserId,
                                            WorkshopId = u.WorkshopId ?? Guid.Empty,
                                            WorkshopName = wItem.Name ?? string.Empty,
                                            BatchId = mr.BatchId,
                                            AssignId = mr.AssignId,
                                            QuantityRequest = mr.QuantityRequest,
                                            Status = mr.Status,
                                            Note = mr.Note,
                                            Date = mr.Date,
                                            Type = mr.Type,
                                            NoteFromQC = mr.NoteFromQC,
                                            ActualReceivedQuantity = mr.ActualReceivedQuantity
                                        };

            if (!string.IsNullOrEmpty(request.Status))
            {
                materialRequestsQuery = materialRequestsQuery
                    .Where(m => string.Equals(m.Status, request.Status, StringComparison.OrdinalIgnoreCase));
            }

            return Result<List<MaterialRequestDTO>>.Success(materialRequestsQuery.ToList());
        }
    }
}
