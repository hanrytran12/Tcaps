using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Request;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.CreateMaterialRequestFromQC
{
    public class CreateMaterialRequestFromQCCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid AssignId { get; set; }
        public string Note { get; set; } = string.Empty;
        [JsonIgnore]
        public string Type { get; set; } = "QcAddMaterial";
        public List<MaterialRequestItemDTO> Items { get; set; } = new();
    }
}
