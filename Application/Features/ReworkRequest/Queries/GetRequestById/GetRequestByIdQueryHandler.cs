using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetRequestById
{
    public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, Application.DTOs.Response.ReworkRequestResponseDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetRequestByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Application.DTOs.Response.ReworkRequestResponseDTO> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.Id == request.ReworkRequestId).FirstOrDefaultAsync(cancellationToken);
            if (reworkRequest is null)
            {
                throw new NotFoundException("ReworkRequest is not found");
            }

            return new Application.DTOs.Response.ReworkRequestResponseDTO
            {
                Id = reworkRequest.Id,
                QcId = reworkRequest.QcId,
                AssignmentId = reworkRequest.AssignmentId,
                DefectiveQuantity = reworkRequest.DefectiveQuantity,
                NoteQc = reworkRequest.NoteQc,
                Status = reworkRequest.Status,
                CreatedAt = reworkRequest.CreatedAt,
                DeliveryDate = reworkRequest.DeliveryDate,
                EndDate = reworkRequest.EndDate,
                NextStepDeliveryDate = reworkRequest.NextStepDeliveryDate
            };
        }
    }
}
