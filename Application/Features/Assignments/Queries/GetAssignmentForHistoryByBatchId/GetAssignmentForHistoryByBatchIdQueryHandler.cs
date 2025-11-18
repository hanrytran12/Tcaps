using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Features.Assignments.Queries.NewFolder;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.Assignments.Queries.GetAssignmentForHistoryByBatchId
{
    public class GetAssignmentForHistoryByBatchIdQueryHandler : IRequestHandler<GetAssignmentForHistoryByBatchIdQuery, Result<List<AssignmentHistoryDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentForHistoryByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<AssignmentHistoryDTO>>> Handle(GetAssignmentForHistoryByBatchIdQuery request, CancellationToken cancellationToken)
        {
            //// 1) Lấy thông tin QC trước (để lấy WorkshopId)
            //var qc = await _context.Users
            //    .Where(u => u.Id == request.QcId)
            //    .Select(u => new { u.Id, u.WorkshopId })
            //    .FirstOrDefaultAsync(cancellationToken);

            //if (qc == null)
            //    return Result<List<AssignmentHistoryDTO>>.Failure("QC not found");

            //// 2) Lấy các staff thuộc cùng workshop với QC
            //var staffInWorkshop = await _context.Users
            //    .Where(u => u.WorkshopId == qc.WorkshopId && u.Role == "Staff")
            //    .Select(u => new { u.Id, u.FullName })
            //    .ToListAsync(cancellationToken);

            //var staffIds = staffInWorkshop.Select(x => x.Id).ToList();

            //// 3) Join Assignments + Productions + Staff theo điều kiện đúng
            //var productions = await (
            //    from a in _context.Assignments
            //    join p in _context.Productions on a.Id equals p.AssignId
            //    join u in _context.Users on p.UserId equals u.Id
            //    where a.BatchId == request.BatchId
            //          && a.WorkshopId == qc.WorkshopId
            //          //&& staffIds.Contains(p.UserId) // Staff phải cùng workshop với QC
            //    select new
            //    {
            //        Assignment = a.Id,
            //        //ProductionId = p.Id,
            //        StaffName = u.FullName,
            //        //QuantityWork = p.Quantity
            //        StaffRoleInDB = u.Role
            //    }
            //).ToListAsync(cancellationToken);

            //// 4) Lấy lỗi UNFIXABLE
            //var defects = await (
            //    from p in _context.Productions
            //    join e in _context.Evaluates on p.Id equals e.ProductionId
            //    join c in _context.ComponentDefects on e.Id equals c.EvaluateId
            //    where c.Status == "Unfixable"
            //    select new
            //    {
            //        ProductionId = p.Id,
            //        ErrorQuantity = c.Quantity
            //    }
            //).ToListAsync(cancellationToken);

            //var errorDictionary = defects
            //    .GroupBy(x => x.ProductionId)
            //    .ToDictionary(
            //        g => g.Key,
            //        g => g.Sum(x => x.ErrorQuantity)
            //    );

            //// 5) Merge dữ liệu thành DTO
            //var result = productions
            //    .GroupBy(x => x.Assignment)
            //    .Select(g => new AssignmentHistoryDTO
            //    {
            //        WorkshopId = g.Key.WorkshopId,
            //        StepOrder = g.Key.StepOrder,
            //        QuantityOrder = g.Key.Quantity,
            //        UnitPrice = g.Key.UnitPrice,
            //        StartDate = g.Key.StartDate,
            //        EndDate = g.Key.EndDate,
            //        ExpectedDeliveryDate = g.Key.ExpectedDeliveryDate,
            //        Status = g.Key.Status,

            //        Items = g
            //            .GroupBy(i => i.StaffName)
            //            .Select(s => new StaffWorksingDTO
            //            {
            //                StaffName = s.Key,
            //                QuantityWork = s.Sum(x => x.QuantityWork),
            //                QuantityError = s.Sum(x =>
            //                    errorDictionary.ContainsKey(x.ProductionId)
            //                        ? errorDictionary[x.ProductionId]
            //                        : 0
            //                )
            //            }).ToList()
            //    })
            //    .ToList();

            // 1. QUERY NHẸ: Lấy WorkshopId của QC
            var qcWorkshopId = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == request.QcId)
                .Select(u => u.WorkshopId)
                .FirstOrDefaultAsync(cancellationToken);

            if (qcWorkshopId == null)
                return Result<List<AssignmentHistoryDTO>>.Failure("QC không tìm thấy hoặc chưa được gán Workshop.");


            // 2. QUERY CHÍNH (Sử dụng LEFT JOIN để giữ lại Assignment chưa có Production)
            var rawData = await (
                from assign in _context.Assignments.AsNoTracking()
                where assign.BatchId == request.BatchId && assign.WorkshopId == qcWorkshopId

                // *** LEFT JOIN: Assignments -> Productions ***
                join prod in _context.Productions.AsNoTracking()
                    on assign.Id equals prod.AssignId into prodsGroup
                from prod in prodsGroup.DefaultIfEmpty() // Chuyển Group Join thành LEFT JOIN

                    // *** LEFT JOIN: Production -> User (Staff) ***
                    // Ta cần dùng ID ảo (Guid.Empty) để EF có thể dịch Left Join.
                join user in _context.Users.AsNoTracking()
                    on (prod != null ? prod.UserId : Guid.Empty) equals user.Id into userGroup
                from user in userGroup.DefaultIfEmpty()

                    // Lọc vai trò (Chỉ lấy Staff HOẶC trường hợp chưa có Production/User)
                    // prod == null: Giữ lại Assignment chưa có Production.
                    // user.Role.ToLower() == "staff": Giữ lại Assignment đã có Production từ Staff.
                where prod == null || (user != null && user.Role.ToLower() == "staff")

                // --- PROJECTION & SUB-QUERY AGGREGATION ---
                select new
                {
                    // Thông tin Assignment (luôn tồn tại)
                    Assignment = assign,

                    // Thông tin Production & Staff (có thể là null)
                    ProductionId = prod != null ? prod.Id : (Guid?)null,
                    StaffName = user != null ? user.FullName : null,
                    QuantityWork = prod != null ? prod.Quantity : 0,

                    // TÍNH TỔNG LỖI (Chỉ tính nếu Production tồn tại)
                    QuantityError = prod == null
                        ? 0m // Nếu chưa có Production, lỗi = 0
                        : _context.Evaluates
                            .Where(e => e.ProductionId == prod.Id)
                            .SelectMany(e => e.ComponentDefects)
                            .Where(cd => cd.Status == "Unfixable")
                            .Sum(cd => (decimal?)cd.Quantity) ?? 0 // SUM và xử lý null
                })
                .ToListAsync(cancellationToken);

            if (!rawData.Any())
                return Result<List<AssignmentHistoryDTO>>.Success(new List<AssignmentHistoryDTO>());

            // 3. IN-MEMORY GROUPING (Sử dụng C# để chuyển dữ liệu phẳng thành cấu trúc cây)
            var result = rawData
                // Group theo Assignment ID
                .GroupBy(x => x.Assignment.Id)
                .Select(g =>
                {
                    var assignmentData = g.First().Assignment;

                    return new AssignmentHistoryDTO
                    {
                        WorkshopId = assignmentData.WorkshopId,
                        StepOrder = assignmentData.StepOrder,
                        QuantityOrder = assignmentData.Quantity,
                        UnitPrice = assignmentData.UnitPrice,
                        StartDate = assignmentData.StartDate,
                        EndDate = assignmentData.EndDate,
                        ExpectedDeliveryDate = assignmentData.ExpectedDeliveryDate,
                        Status = assignmentData.Status,

                        // Group tiếp theo StaffName (có thể là null nếu chưa có Production)
                        Items = g
                            // Loại bỏ trường hợp chỉ có 1 row với StaffName = null khi không có Production nào (nếu có 1 row với StaffName = null, ta vẫn muốn hiển thị nó)
                            // Nếu có Production, nó sẽ nhóm theo tên Staff
                            .GroupBy(x => x.StaffName)
                            .Select(sg => new StaffWorksingDTO
                            {
                                // Nếu StaffName là null, hiển thị "Chưa có dữ liệu"
                                StaffName = sg.Key ?? "Chưa có dữ liệu sản xuất",
                                QuantityWork = sg.Sum(x => x.QuantityWork),
                                QuantityError = sg.Sum(x => (int)x.QuantityError)
                            })
                            .ToList()
                    };
                })
                .OrderBy(x => x.StepOrder)
                .ToList();

            return Result<List<AssignmentHistoryDTO>>.Success(result);
        }
    }
}
