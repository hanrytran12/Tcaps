using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByQcId
{
    public class GetReworkByQcIdQuery : IRequest<List<ReworkRequestDTO>>
    {
        [JsonIgnore]
        public Guid QcId { get; set; }
    }
}
