using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Incomes.Queries.GetMonthlyIncome
{
    public class GetMonthlyIncomeQueryHandler : IRequestHandler<GetMonthlyIncomeQuery, Result<MonthlyIncomeDTO>>
    {
        private readonly IIncomeRepository _incomeRepository;

        public GetMonthlyIncomeQueryHandler(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }
        public async Task<Result<MonthlyIncomeDTO>> Handle(GetMonthlyIncomeQuery request, CancellationToken cancellationToken)
        {
            var incomes = await _incomeRepository.GetIncomeByUserAndMonth(request.StaffId, request.Month, request.Year);

            var grouped = incomes
                .GroupBy(x => x.CreatedAt.Day)
                .Select(g => new DailyIncomeDTO
                {
                    Day = g.Key, //g.Key là ngày lấy được bên trên
                    Total = g.Sum(i => i.TotalPrice)
                })
                .ToDictionary(x => x.Day, x => x.Total);

            var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);
            var dailyLists = new List<DailyIncomeDTO>();

            for(int day = 1; day <= daysInMonth; day++)
            {
                dailyLists.Add(new DailyIncomeDTO
                {
                    Day = day,
                    Total = grouped.ContainsKey(day) ? grouped[day] : 0
                });
            }

            var monthlyIncomeDTO = new MonthlyIncomeDTO
            {
                Month = request.Month,
                Year = request.Year,
                TotalIncome = dailyLists.Sum(x => x.Total),
                DailyIncome = dailyLists
            };

            return Result<MonthlyIncomeDTO>.Success(monthlyIncomeDTO);
        }
    }
}
