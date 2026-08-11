using Domain.Entities;
using MediatR;
using Application.DTOs.Response;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQuery : IRequest<InventoryResponseDTO>
    {
        public Guid Id { get; set; }

        public GetInventoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
