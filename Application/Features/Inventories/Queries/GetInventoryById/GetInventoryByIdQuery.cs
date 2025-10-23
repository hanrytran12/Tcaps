using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQuery : IRequest<Result<Inventory>>
    {
        public Guid Id { get; set; }

        public GetInventoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
