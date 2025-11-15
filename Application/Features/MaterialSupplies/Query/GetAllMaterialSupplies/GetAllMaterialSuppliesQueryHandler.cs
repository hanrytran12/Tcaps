using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialSupplies.Query.GetAllMaterialSupplies
{
    public class GetAllMaterialSuppliesQueryHandler : IRequestHandler<GetAllMaterialSuppliesQuery, Result<List<MaterialSupplyDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetAllMaterialSuppliesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<MaterialSupplyDTO>>> Handle(GetAllMaterialSuppliesQuery request, CancellationToken cancellationToken)
        {
            var query = from s in _context.MaterialSupplies
                        join m in _context.Materials on s.MaterialId equals m.Id
                        join r in _context.MaterialRequests on s.RequestId equals r.Id
                        join b in _context.Batches on r.BatchId equals b.Id
                        join u in _context.Users on r.UserId equals u.Id
                        join w in _context.Workshop on u.WorkshopId equals w.Id
                        select new { s, m, r, b, u, w };

            // Normalize role
            string role = request.Role?.Trim() ?? "";

            // Filter by role
            if (role.Equals("QcTransport", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.s.SupplierId == request.UserId);
            }
            else if (role.Equals("QC", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.r.UserId == request.UserId);
            }
            else if (role.Equals("Lead", StringComparison.OrdinalIgnoreCase) ||
                     role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Lead/Admin xem toàn bộ → không filter
            }
            else
            {
                return Result<List<MaterialSupplyDTO>>.Failure("Vai trò người dùng không hợp lệ.");
            }

            if (request.Status != null)
                query = query.Where(x => x.s.Status == request.Status);

            // chuyển sang DTO
            var list = await query.Select(x => new MaterialSupplyDTO
            {
                Id = x.s.Id,
                RequestId = x.s.RequestId,
                MaterialId = x.s.MaterialId,
                MaterialName = x.m.Name,
                BatchCode = x.b.Code,
                WorkshopName = x.w.Name,
                SupplierId = x.s.SupplierId,
                Quantity = x.s.Quantity,
                Unit = x.s.Unit,
                DateShip = x.s.DateShip,
                Status = x.s.Status
            }).ToListAsync(cancellationToken);

            return Result<List<MaterialSupplyDTO>>.Success(list);
        }
    }
}
