using Application.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Workshop.Command.AddWorkshop
{
    public class AddWorkshopCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public WorkshopType WorkshopType { get; set; }
    }
}
