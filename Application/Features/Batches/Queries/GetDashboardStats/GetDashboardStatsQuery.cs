using Application.DTOs.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Batches.Queries.GetDashboardStats
{
    public class GetDashboardStatsQuery : IRequest<DashboardResultDTO>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }
    }
}
