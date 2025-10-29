using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc
{
    public class ConfirmRequestFromQcCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public decimal ActualReceivedQuantity { get; set; }
        public string NoteFromQC { get; set; } = string.Empty;
    }
}
