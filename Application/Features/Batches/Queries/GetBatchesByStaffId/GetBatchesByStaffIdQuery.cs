using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQuery : IRequest<Result<List<BatchDTO>>>
    {
        public Guid StaffId { get; set; }
    }
}
