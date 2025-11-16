using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest
{
    public class GetAllTaskTransferRequestQuery : IRequest<Result<List<TaskTransferRequestDTO>>>
    {
        public string? Status { get; set; }
    }
}
