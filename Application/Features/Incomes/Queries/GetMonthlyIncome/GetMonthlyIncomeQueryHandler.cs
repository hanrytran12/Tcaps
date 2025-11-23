using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Incomes.Queries.GetMonthlyIncome
{
    public class GetMonthlyIncomeQueryHandler : IRequestHandler<GetMonthlyIncomeQuery, Result<MonthlyIncomeDTO>>
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAppDbContext _context;

        public GetMonthlyIncomeQueryHandler(IIncomeRepository incomeRepository, IAppDbContext context)
        {
            _incomeRepository = incomeRepository;
            _context = context;
        }
        public async Task<Result<MonthlyIncomeDTO>> Handle(GetMonthlyIncomeQuery request, CancellationToken cancellationToken)
        {
            var incomes = await _incomeRepository.GetIncomeByUserAndMonth(request.StaffId, request.Month, request.Year);

            var defects = (
                    from defect in _context.ComponentDefects
                    join eval in _context.Evaluates
                        on defect.EvaluateId equals eval.Id
                    join prod in _context.Productions
                        on eval.ProductionId equals prod.Id
                    where defect.Status == "Unfixable"
                        && prod.UserId == request.StaffId
                        && defect.CreatedAt.Month == request.Month
                        && defect.CreatedAt.Year == request.Year
                    select defect
                ).ToList();

            var defectGrouped = defects
                .GroupBy(d => d.CreatedAt.Day)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(d => d.Quantity)
                );


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

            for (int day = 1; day <= daysInMonth; day++)
            {
                dailyLists.Add(new DailyIncomeDTO
                {
                    Day = day,
                    Total = grouped.ContainsKey(day) ? grouped[day] : 0,
                    QuantityErrors = defectGrouped.ContainsKey(day) ? defectGrouped[day] : 0
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
