using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByWorkshopId
{
    public class GetUserByWorkshopIdQuery : IRequest<UserDTO>
    {
        public Guid WorkshopId { get; set; }

        public GetUserByWorkshopIdQuery(Guid workshopId)
        {
            WorkshopId = workshopId;
        }
    }
}
