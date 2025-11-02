using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Workshop.Queries.GetWorkshopTemplate
{
    public class GetWorkshopTemplateQuery : IRequest<List<WorkshopsDTO>>
    {
    }
}
