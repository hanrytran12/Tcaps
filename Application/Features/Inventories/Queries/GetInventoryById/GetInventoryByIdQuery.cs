using Domain.Entities;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQuery : IRequest<Inventory>
    {
        public Guid Id { get; set; }

        public GetInventoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
