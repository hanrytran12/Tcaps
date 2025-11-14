using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

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
        public string Image { get; set; } = string.Empty;

        public List<ComponentDefectsDTO> Defects { get; set; } = new();
    }
}
