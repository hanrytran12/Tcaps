using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommand : IRequest<Result<Guid>>
    {
        public Guid ProductionId { get; private set; }
        public Guid? UserId { get; private set; }
        public string Note { get; private set; } = string.Empty;
        public int QuantityError { get; private set; }
        public string Image { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public List<ComponentDefectsDTO> Defects { get; set; } = new();
    }
}
