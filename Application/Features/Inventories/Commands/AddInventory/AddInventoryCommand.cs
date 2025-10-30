using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Inventories.Commands.AddInventory
{
    public class AddInventoryCommand : IRequest<Result<Guid>>
    {
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageURL { get; set; }
    }
}
