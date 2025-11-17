using Application.Common;
using Application.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommand : IRequest<Result<Guid>>
    {
        public Guid ProductionId { get; set; }
        [JsonIgnore]
        public Guid? UserId { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int QuantityError { get; set; }
        public int QuantitySucess { get; set; }
        public List<IFormFile> Image { get; set; }

        public List<ComponentDefectsDTO> Defects { get; set; } = new();
    }
}
