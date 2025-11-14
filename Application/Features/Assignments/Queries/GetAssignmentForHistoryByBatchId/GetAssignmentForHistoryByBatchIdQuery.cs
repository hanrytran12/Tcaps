using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.NewFolder
{
    public class GetAssignmentForHistoryByBatchIdQuery : IRequest<Result<List<AssignmentHistoryDTO>>>
    {
        public Guid QcId { get; set; }
        public Guid BatchId { get; set; }
    }
}
