using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Assignments.Commands.UpdateReadyForTransfer
{
    public class UpdateReadyForTransferCommand : IRequest<Result<Guid>>
    {
        public Guid AssignmentId { get; set; }
        [JsonIgnore]
        public Guid QcId { get; set; }
    }
}
