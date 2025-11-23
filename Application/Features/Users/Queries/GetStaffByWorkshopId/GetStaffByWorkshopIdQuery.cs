using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Users.Queries.GetStaffByWorkshopId
{
    public class GetStaffByWorkshopIdQuery : IRequest<List<UsersDTO>>
    {
        public Guid WorkshopId { get; set; }
        public GetStaffByWorkshopIdQuery(Guid workshopId)
        {
            WorkshopId = workshopId;
        }
    }
}
