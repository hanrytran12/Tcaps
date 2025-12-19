using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.FinalTransferRequest.Queries.GetAllFinalTransferRequest
{
    public class GetAllFinalTransferRequestQuery : IRequest<List<FinalTransferRequestDTO>>
    {
    }
}
