using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Inventories.Commands.AddInventory
{
    public class AddInventoryCommand : IRequest<Result<Guid>>
    {
        public string MaterialName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageURL { get; set; }

        public string? NameMaterialDescription { get; set; }
        public string? UnitMaterial { get; set; }
    }
}
