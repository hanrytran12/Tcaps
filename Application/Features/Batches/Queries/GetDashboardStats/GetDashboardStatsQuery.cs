using Application.DTOs.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Batches.Queries.GetDashboardStats
{
    public class GetDashboardStatsQuery : IRequest<DashboardResultDTO>
    {
        [Required]
        public Guid ProductId { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
