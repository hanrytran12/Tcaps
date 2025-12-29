using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetTaskProgressByQCId
{
    public class GetTaskProgressByQCIdQuery : IRequest<List<TaskProgressDTO>>
    {
        [JsonIgnore]
        public Guid QcId { get; set; }
    }
}
