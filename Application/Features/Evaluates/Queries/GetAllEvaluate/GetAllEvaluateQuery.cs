using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetAllEvaluate
{
    public class GetAllEvaluateQuery : IRequest<List<EvaluateDTO>>
    {
    }
}
