using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs;
using Application.DTOs.Request;
using MediatR;

namespace Application.Features.Evaluates.Commands.UpdateEvaluate
{
    public class UpdateEvaluateCommand : IRequest<Result<ResponseDTO>>
    {
        public Guid Id { get; set; }
        public string Note { get; set; } = string.Empty;
        public int QuantityError { get; set; }
        public string Image { get; set; } = string.Empty;

        public List<UpdateComponentDefectDTO> Defects { get; set; } = new();
    }
}
