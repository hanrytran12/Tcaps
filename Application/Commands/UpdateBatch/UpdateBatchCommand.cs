using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.UpdateBatch
{
    public class UpdateBatchCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
