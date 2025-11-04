using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Incomes.Command.AddIncome
{
    public class AddIncomeCommand : IRequest<Result<Guid>>
    {
        public Guid ProductionId { get; set; }
        public Guid UserId { get; set; }
    }
}
