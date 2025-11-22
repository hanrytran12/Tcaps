using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive
{
    public class TotalQuantityReceiveQuery : IRequest<Result<int>>
    {
        public Guid QcId { get; set; }
        public Guid BatchId { get; set; }
    }
}
