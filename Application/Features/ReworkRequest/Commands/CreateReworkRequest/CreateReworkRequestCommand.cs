using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.ReworkRequest.Commands.CreateReworkRequest
{
    public class CreateReworkRequestCommand : IRequest<Result<Guid>>
    {
        public Guid AssigmentId { get; set; }
        [JsonIgnore]
        public Guid QCId { get; set; }
        public decimal DefectiveQuantity { get; set; }
        public string NoteQc { get; set; } = string.Empty;
    }
}
