using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
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

                        // 💡 JOIN 1: Lấy thông tin Người Cung cấp (Supplier)
                        join su in _context.Users.AsNoTracking() on s.SupplierId equals su.Id into supplierGroup
                        from supplierUser in supplierGroup.DefaultIfEmpty()

                            // 💡 JOIN 2: Lấy thông tin Người Yêu cầu/Nhận (Receiver)
                        join ru in _context.Users.AsNoTracking() on r.UserId equals ru.Id into receiverGroup
                        from receiverUser in receiverGroup.DefaultIfEmpty()

                            // 💡 JOIN 3: Workshop (dựa trên người yêu cầu, thường là QC Workshop)
                        join w in _context.Workshop.AsNoTracking() on receiverUser.WorkshopId equals w.Id into workshopGroup
                        from wItem in workshopGroup.DefaultIfEmpty() // Thêm DefaultIfEmpty()
                        select new { s, m, r, b, supplierUser, receiverUser, wItem };

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
                throw new BadRequestException("Vai trò người dùng không hợp lệ.");
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
                WorkshopId = x.receiverUser.WorkshopId ?? Guid.Empty,
                WorkshopName = x.wItem.Name,
                SupplierId = x.s.SupplierId,
                SupplierName = x.supplierUser.FullName ?? string.Empty,
                ReceiverName = x.receiverUser.FullName,
                QuantitySend = x.s.QuantitySend,
                QuantityReceive = x.s.QuantityReceive ?? 0,
                Unit = x.s.Unit,
                DateShip = x.s.DateShip,
                Status = x.s.Status
            }).ToListAsync(cancellationToken);

            return Result<List<MaterialSupplyDTO>>.Success(list);
        }
    }
}
