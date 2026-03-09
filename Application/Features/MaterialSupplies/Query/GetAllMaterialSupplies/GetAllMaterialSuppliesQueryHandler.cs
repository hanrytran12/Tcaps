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
                        join p in _context.Products on b.ProductId equals p.Id

                        // 💡 JOIN 1: Lấy thông tin Người Cung cấp (Supplier)
                        join su in _context.Users.AsNoTracking() on s.SupplierId equals su.Id into supplierGroup
                        from supplierUser in supplierGroup.DefaultIfEmpty()

                            // 💡 JOIN 2: Lấy thông tin Người Yêu cầu/Nhận (Receiver)
                        join ru in _context.Users.AsNoTracking() on r.UserId equals ru.Id into receiverGroup
                        from receiverUser in receiverGroup.DefaultIfEmpty()

                            // 💡 JOIN 3: Workshop (dựa trên người yêu cầu, thường là QC Workshop)
                        join w in _context.Workshop.AsNoTracking() on receiverUser.WorkshopId equals w.Id into workshopGroup
                        from wItem in workshopGroup.DefaultIfEmpty() // Thêm DefaultIfEmpty()
                        select new { s, m, r, b, p, supplierUser, receiverUser, wItem };

            // Normalize role
            string role = request.Role?.Trim() ?? "";

            // Filter by role
            if (role.Equals("QcTransport", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.s.SupplierId == request.UserId);
            }
            else if (role.Equals("QC", StringComparison.OrdinalIgnoreCase))
            {

            }
            else if (role.Equals("Lead", StringComparison.OrdinalIgnoreCase))
            {
                // Lead/Admin xem toàn bộ → không filter
            }
            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var qcTransportUserIds = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.Role == "QCTransport")
                    .Select(u => u.Id)
                    .ToListAsync(cancellationToken);

                query = query.Where(q => qcTransportUserIds.Contains(q.s.SupplierId));
            }
            else if (role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
            {
                var staffUser = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                if (staffUser?.WorkshopId != null)
                {
                    query = query.Where(x => x.receiverUser.WorkshopId == staffUser.WorkshopId);
                }
                else
                {
                    return Result<List<MaterialSupplyDTO>>.Success(new List<MaterialSupplyDTO>());
                }
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
                ProductCode = x.p.Code,
                ProductName = x.p.Name,
                WorkshopId = x.receiverUser.WorkshopId ?? Guid.Empty,
                WorkshopName = x.wItem.Name,
                SupplierId = x.s.SupplierId,
                SupplierName = x.supplierUser.FullName ?? string.Empty,
                ReceiverName = x.receiverUser.FullName,
                QuantitySend = x.s.QuantitySend,
                QuantityReceive = x.s.QuantityReceive ?? 0,
                Unit = x.s.Unit,
                DateShip = x.s.DateShip,
                DateReceive = x.s.DateReceive,
                Status = x.s.Status,
                Note = x.s.Note,
                LeadNote = x.s.LeadNote
            }).ToListAsync(cancellationToken);

            return Result<List<MaterialSupplyDTO>>.Success(list);
        }
    }
}
