using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByWorkshopId
{
    public class GetUserByWorkshopIdQuery : IRequest<User>
    {
        public Guid WorkshopId { get; set; }

        public GetUserByWorkshopIdQuery(Guid workshopId)
        { WorkshopId = workshopId; }
    }
}
