using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQcTransport
{
    public class GetMaterialRequestForQcTransportQueryHandler : IRequestHandler<GetMaterialRequestForQcTransportQuery, Result<MaterialRequestDTO>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IAppDbContext _context;

        public GetMaterialRequestForQcTransportQueryHandler(IMaterialRequestRepository materialRequestRepository, IAppDbContext context)
        {
            _materialRequestRepository = materialRequestRepository;
            _context = context;
        }
        public async Task<Result<MaterialRequestDTO>> Handle(GetMaterialRequestForQcTransportQuery request, CancellationToken cancellationToken)
        {
            var materialRequestsQuery = from mr in _context.MaterialRequests.AsNoTracking()

                                        join m in _context.Materials.AsNoTracking()
                                        on mr.MaterialId equals m.Id

                                        join u in _context.Users.AsNoTracking()
                                            on mr.UserId equals u.Id

                                        join w in _context.Workshop.AsNoTracking()
                                            on u.WorkshopId equals w.Id into workshopGroup
                                        from wItem in workshopGroup.DefaultIfEmpty()
                                        where mr.Id == request.MaterialRequestId
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

            var result = await materialRequestsQuery.FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu vật liệu với ID đã cung cấp.");
            }

            return Result<MaterialRequestDTO>.Success(result);
        }
    }
}
