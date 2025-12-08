using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.TaskTransferRequests.Queries.GetByMaterialRequestIdOrAssignTransferId
{
    public class GetByMaterialRequestIdOrAssignTransferIdQuery : IRequest<TaskTransferRequestDTO>
    {
        public Guid RequestId { get; set; }
    }
}
