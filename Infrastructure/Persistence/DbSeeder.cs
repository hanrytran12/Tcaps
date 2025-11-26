using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAllAsync(AppDbContext context)
        {
            await SeedWorkshopsAsync(context);
            await SeedUsersAsync(context);
            await SeedMaterialsAsync(context);
            await SeedProductsAsync(context);
            await SeedInventoriesAsync(context); // <-- Lỗi cũ ở đây
            await SeedBatchesAsync(context);
            await SeedAssignmentsAsync(context);
            await SeedMaterialRequestsAsync(context);
            await SeedMaterialUseAsync(context);
            await SeedMaterialWorkshopsAsync(context);
            await SeedProductionsAsync(context);
            await SeedEvaluatesAndDefectsAsync(context);
            await SeedIncomesAsync(context);
            await SeedNotificationsAsync(context);
            await SeedTransferRequestsAsync(context);

            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 1. WORKSHOPS
        // ==========================================
        private static async Task SeedWorkshopsAsync(AppDbContext context)
        {
            if (await context.Workshop.AnyAsync()) return;

            var workshops = new List<Workshop>
            {
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000001", new { Name = "Cắt laser (nếu có)", Description = "Công đoạn Cắt laser (nếu có)", StepOrder = 1 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000002", new { Name = "Cắt vải ép keo", Description = "Công đoạn Cắt vải ép keo", StepOrder = 2 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000003", new { Name = "Dán vải", Description = "Công đoạn Dán vải", StepOrder = 3 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000004", new { Name = "Cắt vải ra đủ bộ", Description = "Công đoạn Cắt vải ra đủ bộ", StepOrder = 4 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000005", new { Name = "In + thêu", Description = "Công đoạn In + thêu", StepOrder = 5 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000006", new { Name = "Làm chỏm", Description = "Công đoạn Làm chỏm", StepOrder = 6 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000007", new { Name = "Làm kết", Description = "Công đoạn Làm kết", StepOrder = 7 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000008", new { Name = "Đóng nút bấm đuôi (nếu có)", Description = "Công đoạn Đóng nút bấm đuôi (nếu có)", StepOrder = 8 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000009", new { Name = "Xỏ cây dựng + vắt sổ", Description = "Công đoạn Xỏ cây dựng + vắt sổ", StepOrder = 9 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000010", new { Name = "Vào nón + vào đai", Description = "Công đoạn Vào nón + vào đai", StepOrder = 10 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000011", new { Name = "Trần đầu nón", Description = "Công đoạn Trần đầu nón", StepOrder = 11 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000012", new { Name = "May đuôi khóa vào nón (nếu có)", Description = "Công đoạn May đuôi khóa vào nón (nếu có)", StepOrder = 12 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000013", new { Name = "May tem", Description = "Công đoạn May tem", StepOrder = 13 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000014", new { Name = "Cắt chỉ dư + xỏ tem thẻ bài", Description = "Công đoạn Cắt chỉ dư + xỏ tem thẻ bài", StepOrder = 14 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000015", new { Name = "Ủi nón", Description = "Công đoạn Ủi nón", StepOrder = 15 }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000016", new { Name = "Gấp cây + đóng bịch", Description = "Công đoạn Gấp cây + đóng bịch", StepOrder = 16 })
            };
            await context.Workshop.AddRangeAsync(workshops);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 2. USERS
        // ==========================================
        private static async Task SeedUsersAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;
            var ph = new PasswordHasher();
            var pass = ph.Hash("12345678");

            var users = new List<User>();

            User U(string id, string wsId, string role, string name, string email, string phone)
            {
                var u = User.Create(Guid.Parse(wsId), role, name, email, pass, phone);
                SetProperty(u, "Status", "Active");
                SetProperty(u, "IsQcTransport", role == "QCTransport");
                SetProperty(u, "CreatedAt", DateTime.Now);
                SetProperty(u, "Id", Guid.Parse(id));
                return u;
            }

            users.Add(U("A0000000-0000-0000-0000-000000000001", "A1C9B3A0-4F12-4E81-B17B-000000000001", "Admin", "Nguyễn Văn An", "admin@tcaps.com", "0901234567"));
            users.Add(U("A0000000-0000-0000-0000-000000000002", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Lead", "Trần Thị Bình", "lead@tcaps.com", "0902345678"));
            users.Add(U("A0000000-0000-0000-0000-000000000003", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Lê Văn Cường", "staff.cuong@tcaps.com", "0903456789"));
            users.Add(U("A0000000-0000-0000-0000-000000000004", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Phạm Thị Dung", "staff.dung@tcaps.com", "0904567890"));
            users.Add(U("A0000000-0000-0000-0000-000000000005", "A1C9B3A0-4F12-4E81-B17B-000000000003", "QCTransport", "Nguyễn Thị Hạnh", "qctransport@company.com", "0906789012"));

            // QC 16 steps
            for (int i = 1; i <= 16; i++)
            {
                string suffix = i.ToString("D2");
                string wsId = $"A1C9B3A0-4F12-4E81-B17B-0000000000{suffix}";
                string uId = $"A0000000-0000-0000-0000-0000000000{i + 5:D2}";
                string phone = $"09010001{suffix}";
                string name = $"QC Step {i}";
                string email = $"qc{i}@tcaps.com";

                if (i == 1) { name = "QC Xưởng Cắt Laser"; email = "qc.laser@tcaps.com"; }
                if (i == 2) { name = "Hoàng Văn Em"; email = "qc.catkeo@tcaps.com"; }
                if (i == 3) { name = "Khánh Nguyệt"; email = "qc.danvai@tcaps.com"; }
                if (i == 4) { name = "QC Xưởng Cắt Đủ Bộ"; email = "qc.catdubo@tcaps.com"; }
                if (i == 5) { name = "Anh Dũng"; email = "qc.intheu@tcaps.com"; }

                users.Add(U(uId, wsId, "QC", name, email, phone));
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 3. MATERIALS
        // ==========================================
        private static async Task SeedMaterialsAsync(AppDbContext context)
        {
            if (await context.Materials.AnyAsync()) return;

            var mats = new List<Material>
            {
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000001", new { Name = "Vải cotton loại A", Description = "Vải cotton cao cấp dùng làm nón", Quantity = 1000m, Price = 50000m, Unit = "Mét" }),
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000002", new { Name = "Sợi nylon 210D", Description = "Sợi nylon độ bền cao", Quantity = 500m, Price = 80000m, Unit = "Kg" }),
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000003", new { Name = "Dây viền polyester", Description = "Dây viền trang trí nón", Quantity = 300m, Price = 30000m, Unit = "Kg" }),
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000004", new { Name = "Keo dán vải", Description = "Keo chuyên dụng ép vải", Quantity = 200m, Price = 120000m, Unit = "Lít" }),
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000005", new { Name = "Mút xốp lót nón", Description = "Mút xốp êm ái lót bên trong", Quantity = 800m, Price = 25000m, Unit = "Miếng" }),
                CreateEntity<Material>("B0000000-0000-0000-0000-000000000006", new { Name = "Khóa cài nón", Description = "Khóa cài nhựa chắc chắn", Quantity = 1000m, Price = 5000m, Unit = "Cái" })
            };
            await context.Materials.AddRangeAsync(mats);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 4. PRODUCTS
        // ==========================================
        private static async Task SeedProductsAsync(AppDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var products = new List<Product>
            {
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000001", new { Code = "NON-001", Name = "Nón bảo hiểm 3/4 đầu", Image = "/images/products/helmet_34.jpg", Description = "Nón bảo hiểm 3/4 đầu có kính", IsDeleted = false }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000002", new { Code = "NON-002", Name = "Nón bảo hiểm fullface", Image = "/images/products/helmet_fullface.jpg", Description = "Nón bảo hiểm fullface nguyên đầu", IsDeleted = false }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000003", new { Code = "NON-003", Name = "Nón bảo hiểm nửa đầu", Image = "/images/products/helmet_half.jpg", Description = "Nón bảo hiểm nửa đầu thời trang", IsDeleted = false }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000004", new { Code = "NON-004", Name = "Nón bảo hiểm trẻ em", Image = "/images/products/helmet_kids.jpg", Description = "Nón bảo hiểm dành cho trẻ em", IsDeleted = false }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000005", new { Code = "NON-005", Name = "Nón bảo hiểm thể thao", Image = "/images/products/helmet_sport.jpg", Description = "Nón bảo hiểm thể thao Motocross", IsDeleted = false })
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 5. INVENTORIES
        // ==========================================
        private static async Task SeedInventoriesAsync(AppDbContext context)
        {
            if (await context.Inventories.AnyAsync()) return;
            // Dùng DateTime.Now để an toàn, SetProperty sẽ lo việc chuyển đổi
            var today = DateTime.Now;

            var invs = new List<Inventory>
            {
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000001", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), Quantity = 800m, Date = today, Price = 50000m, ImageURL = "https://example.com/images/inventory1.jpg" }),
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000002", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000002"), Quantity = 400m, Date = today, Price = 80000m, ImageURL = "https://example.com/images/inventory2.jpg" }),
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000003", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000003"), Quantity = 280m, Date = today, Price = 30000m, ImageURL = "https://example.com/images/inventory3.jpg" }),
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000004", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000004"), Quantity = 150m, Date = today, Price = 120000m, ImageURL = "https://example.com/images/inventory4.jpg" }),
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000005", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000005"), Quantity = 750m, Date = today, Price = 25000m, ImageURL = "https://example.com/images/inventory5.jpg" }),
                CreateEntity<Inventory>("10000000-0000-0000-0000-000000000006", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000006"), Quantity = 900m, Date = today, Price = 5000m, ImageURL = "https://example.com/images/inventory6.jpg" })
            };
            await context.Inventories.AddRangeAsync(invs);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 6. BATCHES
        // ==========================================
        private static async Task SeedBatchesAsync(AppDbContext context)
        {
            if (await context.Batches.AnyAsync()) return;
            var batches = new List<Batch>
            {
                // Dùng DateTime cho lành, helper sẽ tự convert
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000001", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000001"), Code = "BATCH-2025-001", Quantity = 100m, StartDate = DateTime.Parse("2025-10-01"), EndDate = DateTime.Parse("2025-11-12"), Status = "Planned", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000002", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000002"), Code = "BATCH-2025-002", Quantity = 50m, StartDate = DateTime.Parse("2025-11-05"), EndDate = DateTime.Parse("2025-11-20"), Status = "Planned", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000003", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000001"), Code = "BATCH-2025-003", Quantity = 150m, StartDate = DateTime.Parse("2025-10-10"), EndDate = DateTime.Parse("2025-11-02"), Status = "Completed", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000004", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000003"), Code = "BATCH-2025-004", Quantity = 80m, StartDate = DateTime.Parse("2025-11-12"), EndDate = DateTime.Parse("2025-11-30"), Status = "InProgress", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000005", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000004"), Code = "BATCH-2025-005", Quantity = 120m, StartDate = DateTime.Parse("2025-11-01"), EndDate = DateTime.Parse("2025-12-28"), Status = "InProgress", IsDeleted = false, CreatedAt = DateTime.Now })
            };
            await context.Batches.AddRangeAsync(batches);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 7. ASSIGNMENTS
        // ==========================================
        private static async Task SeedAssignmentsAsync(AppDbContext context)
        {
            if (await context.Assignments.AnyAsync()) return;
            var assigns = new List<Assignment>
            {
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000001", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), StepOrder = 1, Quantity = 100m, UnitPrice = 10000m, StartDate = DateTime.Parse("2025-11-01"), EndDate = DateTime.Parse("2025-11-03"), ExpectedDeliveryDate = DateTime.Parse("2025-11-03"), RequiresMaterialDelivery = true, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000002", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"), StepOrder = 2, Quantity = 100m, UnitPrice = 15000m, StartDate = DateTime.Parse("2025-11-04"), EndDate = DateTime.Parse("2025-11-06"), ExpectedDeliveryDate = DateTime.Parse("2025-11-06"), RequiresMaterialDelivery = true, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000003", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), StepOrder = 3, Quantity = 100m, UnitPrice = 20000m, StartDate = DateTime.Parse("2025-11-07"), EndDate = DateTime.Parse("2025-11-09"), ExpectedDeliveryDate = DateTime.Parse("2025-11-09"), RequiresMaterialDelivery = true, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000004", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000004"), StepOrder = 4, Quantity = 100m, UnitPrice = 12000m, StartDate = DateTime.Parse("2025-11-10"), EndDate = DateTime.Parse("2025-11-12"), ExpectedDeliveryDate = DateTime.Parse("2025-11-12"), RequiresMaterialDelivery = false, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000005", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000001"), StepOrder = 1, Quantity = 50m, UnitPrice = 10000m, StartDate = DateTime.Parse("2025-11-05"), EndDate = DateTime.Parse("2025-11-07"), ExpectedDeliveryDate = DateTime.Parse("2025-11-07"), RequiresMaterialDelivery = true, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000006", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"), StepOrder = 2, Quantity = 50m, UnitPrice = 15000m, StartDate = DateTime.Parse("2025-11-08"), EndDate = DateTime.Parse("2025-11-10"), ExpectedDeliveryDate = DateTime.Parse("2025-11-10"), RequiresMaterialDelivery = true, Status = "InProgress", CreatedAt = DateTime.Now }),
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000007", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000005"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), StepOrder = 16, Quantity = 120m, UnitPrice = 8000m, StartDate = DateTime.Parse("2025-11-06"), EndDate = DateTime.Parse("2025-11-20"), ExpectedDeliveryDate = DateTime.Parse("2025-11-06"), RequiresMaterialDelivery = false, Status = "InProgress", CreatedAt = DateTime.Now })
            };
            await context.Assignments.AddRangeAsync(assigns);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 8. MATERIAL REQUESTS
        // ==========================================
        private static async Task SeedMaterialRequestsAsync(AppDbContext context)
        {
            if (await context.MaterialRequests.AnyAsync()) return;
            var reqs = new List<MaterialRequest>
            {
                CreateEntity<MaterialRequest>("F0000000-0000-0000-0000-000000000001", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), QuantityRequest = 200m, Status = "Đã duyệt", Note = "Cần gấp", Date = DateTime.Parse("2025-11-01"), Type = "LeadExport" }),
                CreateEntity<MaterialRequest>("F0000000-0000-0000-0000-000000000002", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000004"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000002"), QuantityRequest = 50m, Status = "Đã duyệt", Note = "Keo ép vải", Date = DateTime.Parse("2025-11-04"), Type = "QcAddMaterial" }),
                CreateEntity<MaterialRequest>("F0000000-0000-0000-0000-000000000003", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000005"), QuantityRequest = 100m, Status = "Chờ duyệt", Note = "Sợi nylon", Date = DateTime.Parse("2025-11-05"), Type = "QcAddMaterial" })
            };
            await context.MaterialRequests.AddRangeAsync(reqs);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 9. MATERIAL USE
        // ==========================================
        private static async Task SeedMaterialUseAsync(AppDbContext context)
        {
            if (await context.MaterialUse.AnyAsync()) return;
            var uses = new List<MaterialUse>
            {
                CreateEntity<MaterialUse>("20000000-0000-0000-0000-000000000001", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), QuantityDivide = 200m, QuantityStaffUse = 190m, ReconciledQuantity = 185m, QuantityRequest = 200m, Date = DateTime.Parse("2025-11-02") }),
                CreateEntity<MaterialUse>("20000000-0000-0000-0000-000000000002", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000004"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000002"), QuantityDivide = 50m, QuantityStaffUse = 45m, ReconciledQuantity = 43m, QuantityRequest = 50m, Date = DateTime.Parse("2025-11-05") }),
                CreateEntity<MaterialUse>("20000000-0000-0000-0000-000000000003", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000005"), QuantityDivide = 100m, QuantityStaffUse = 95m, ReconciledQuantity = 92m, QuantityRequest = 100m, Date = DateTime.Parse("2025-11-06") }),
                CreateEntity<MaterialUse>("20000000-0000-0000-0000-000000000004", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000003"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000005"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000007"), QuantityDivide = 200m, QuantityStaffUse = 0m, ReconciledQuantity = 0m, QuantityRequest = 0m, Date = DateTime.Parse("2025-11-02") })
            };
            await context.MaterialUse.AddRangeAsync(uses);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 10. MATERIAL WORKSHOPS
        // ==========================================
        private static async Task SeedMaterialWorkshopsAsync(AppDbContext context)
        {
            if (await context.MaterialWorkshops.AnyAsync()) return;
            var mw = new List<MaterialWorkshop>
            {
                CreateEntity<MaterialWorkshop>("30000000-0000-0000-0000-000000000001", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), SupplierId = Guid.Parse("A0000000-0000-0000-0000-000000000007"), QuantitySend = 200m, QuantityReceive = 190m, ShipDate = DateTime.Parse("2025-11-01"), Status = "Confirmed", CreatedAt = DateTime.Now }),
                CreateEntity<MaterialWorkshop>("30000000-0000-0000-0000-000000000002", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000002"), SupplierId = Guid.Parse("A0000000-0000-0000-0000-000000000007"), QuantitySend = 50m, QuantityReceive = 45m, ShipDate = DateTime.Parse("2025-11-04"), Status = "Confirmed", CreatedAt = DateTime.Now }),
                CreateEntity<MaterialWorkshop>("30000000-0000-0000-0000-000000000003", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000004"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000003"), SupplierId = Guid.Parse("A0000000-0000-0000-0000-000000000008"), QuantitySend = 100m, QuantityReceive = 95m, ShipDate = DateTime.Parse("2025-11-05"), Status = "Confirmed", CreatedAt = DateTime.Now }),
                CreateEntity<MaterialWorkshop>("A0000000-0000-0000-0000-000000000015", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), AssignId = Guid.Parse("F91BF72A-77D2-4CD5-9DC1-D1E965CDEADC"), QuantitySend = 100m, QuantityReceive = 95m, ShipDate = DateTime.Parse("2025-11-22"), CreatedAt = DateTime.Parse("2025-11-20"), Status = "Approved" })
            };
            await context.MaterialWorkshops.AddRangeAsync(mw);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 11. PRODUCTIONS
        // ==========================================
        private static async Task SeedProductionsAsync(AppDbContext context)
        {
            if (await context.Productions.AnyAsync()) return;
            var prods = new List<Production>
            {
                CreateEntity<Production>("40000000-0000-0000-0000-000000000001", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Quantity = 100m, Date = DateTime.Parse("2025-11-03"), Status = "Passed" }),
                CreateEntity<Production>("40000000-0000-0000-0000-000000000002", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Quantity = 50m, Date = DateTime.Parse("2025-11-06"), Status = "Passed" }),
                CreateEntity<Production>("40000000-0000-0000-0000-000000000003", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000005"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Quantity = 50m, Date = DateTime.Parse("2025-11-07"), Status = "Passed" }),
                CreateEntity<Production>("40000000-0000-0000-0000-000000000004", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000005"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Quantity = 120m, Date = DateTime.Parse("2025-11-08"), Status = "Rework" })
            };
            await context.Productions.AddRangeAsync(prods);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 12. EVALUATES & DEFECTS
        // ==========================================
        private static async Task SeedEvaluatesAndDefectsAsync(AppDbContext context)
        {
            if (await context.Evaluates.AnyAsync()) return;
            var evas = new List<Evaluate>
            {
                CreateEntity<Evaluate>("50000000-0000-0000-0000-000000000001", new { ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), Status = "Đạt", Note = "Tốt", QuantityError = 0m, QuantitySuccess = 100m, Image = "/images/qc/qc_001.jpg", CreatedAt = DateTime.Now }),
                CreateEntity<Evaluate>("50000000-0000-0000-0000-000000000002", new { ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000003"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), Status = "Đạt", Note = "Fullface ok", QuantityError = 0m, QuantitySuccess = 50m, Image = "/images/qc/qc_002.jpg", CreatedAt = DateTime.Now }),
                CreateEntity<Evaluate>("50000000-0000-0000-0000-000000000003", new { ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000004"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), Status = "Có lỗi", Note = "Lỗi nhỏ", QuantityError = 5m, QuantitySuccess = 115m, Image = "/images/qc/qc_003.jpg", CreatedAt = DateTime.Now })
            };
            await context.Evaluates.AddRangeAsync(evas);
            await context.SaveChangesAsync(CancellationToken.None);

            if (await context.ComponentDefects.AnyAsync()) return;
            var defects = new List<ComponentDefect>
            {
                CreateEntity<ComponentDefect>("60000000-0000-0000-0000-000000000001", new { EvaluateId = Guid.Parse("50000000-0000-0000-0000-000000000003"), DefectType = "Vết xước nhỏ", Serverity = "Thấp", Description = "Vết xước", Solution = "Đánh bóng", Quantity = 3, CreatedAt = DateTime.Now, Status = "Đã xử lý" }),
                CreateEntity<ComponentDefect>("60000000-0000-0000-0000-000000000002", new { EvaluateId = Guid.Parse("50000000-0000-0000-0000-000000000003"), DefectType = "Khóa cài lỏng", Serverity = "Trung bình", Description = "Lỏng", Solution = "Chỉnh lại", Quantity = 2, CreatedAt = DateTime.Now, Status = "Đang xử lý" })
            };
            await context.ComponentDefects.AddRangeAsync(defects);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 13. INCOMES
        // ==========================================
        private static async Task SeedIncomesAsync(AppDbContext context)
        {
            if (await context.Incomes.AnyAsync()) return;
            var incomes = new List<Income>
            {
                CreateEntity<Income>("70000000-0000-0000-0000-000000000001", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Quantity = 100m, TotalPrice = 1000000m, CreatedAt = DateTime.Now }),
                CreateEntity<Income>("70000000-0000-0000-0000-000000000002", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Quantity = 50m, TotalPrice = 750000m, CreatedAt = DateTime.Now }),
                CreateEntity<Income>("70000000-0000-0000-0000-000000000003", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000003"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Quantity = 50m, TotalPrice = 500000m, CreatedAt = DateTime.Now }),
                CreateEntity<Income>("70000000-0000-0000-0000-000000000004", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000005"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000004"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Quantity = 120m, TotalPrice = 960000m, CreatedAt = DateTime.Now })
            };
            await context.Incomes.AddRangeAsync(incomes);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 14. NOTIFICATIONS
        // ==========================================
        private static async Task SeedNotificationsAsync(AppDbContext context)
        {
            if (await context.Notifications.AnyAsync()) return;
            var notis = new List<Notification>
            {
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000001", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Title = "Chào mừng", Message = "Welcome", Type = "System", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000002", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Title = "Nhiệm vụ mới", Message = "Lô BATCH-2025-001", Type = "Task", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000003", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Title = "Nhiệm vụ mới", Message = "Lô BATCH-2025-002", Type = "Task", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000004", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), Title = "Yêu cầu kiểm tra", Message = "Check QC", Type = "QC", IsRead = true, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000005", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000002"), Title = "Báo cáo tuần", Message = "Nộp báo cáo", Type = "Report", IsRead = true, CreatedAt = DateTime.Now })
            };
            await context.Notifications.AddRangeAsync(notis);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 15. TRANSFER REQUESTS & WORKSHOP INVENTORY
        // ==========================================
        private static async Task SeedTransferRequestsAsync(AppDbContext context)
        {
            if (!await context.AssignmentTransferRequests.AnyAsync())
            {
                var atrs = new List<AssignmentTransferRequest>
                {
                    CreateEntity<AssignmentTransferRequest>("a1b2c3d4-e5f6-7890-1234-56789abcdef0", new { AssignmentId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), CompletedQuantity = 50m, Status = "PendingApproval", Note = "Yêu cầu chuyển 1", CreatedAt = DateTime.Now }),
                    CreateEntity<AssignmentTransferRequest>("b2c3d4e5-f6a1-8901-2345-6789abcdef01", new { AssignmentId = Guid.Parse("E0000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), CompletedQuantity = 75m, Status = "PendingApproval", Note = "Yêu cầu chuyển 2", CreatedAt = DateTime.Now })
                };
                await context.AssignmentTransferRequests.AddRangeAsync(atrs);
            }

            if (!await context.TaskTransferRequests.AnyAsync())
            {
                var ttrs = new List<TaskTransferRequest>
                {
                    CreateEntity<TaskTransferRequest>("A3F2504E-4F89-11D3-9A0C-0305E82C3301", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000001"), QcTransportId = Guid.Parse("A0000000-0000-0000-0000-000000000006"), Status = "Pending", Note = "Test 1", CreatedAt = DateTime.Now }),
                    CreateEntity<TaskTransferRequest>("B3F2504E-4F89-11D3-9A0C-0305E82C3302", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"), QcTransportId = Guid.Parse("A0000000-0000-0000-0000-000000000006"), Status = "Approved", Note = "Test 2", CreatedAt = DateTime.Now, ApprovedAt = DateTime.Now }),
                    CreateEntity<TaskTransferRequest>("C3F2504E-4F89-11D3-9A0C-0305E82C3303", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000002"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), QcTransportId = Guid.Parse("A0000000-0000-0000-0000-000000000007"), Status = "Approved", Note = "Test 3", CreatedAt = DateTime.Now })
                };
                await context.TaskTransferRequests.AddRangeAsync(ttrs);
            }

            if (!await context.WorkshopInventory.AnyAsync())
            {
                var winvs = new List<WorkshopInventory>
                {
                    CreateEntity<WorkshopInventory>("01B2C3D4-E5F6-7890-ABCD-EF1234567890", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000001"), MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), Quantity = 100m }),
                    CreateEntity<WorkshopInventory>("02C3D4E5-F6A7-8901-BCDE-F12345678901", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000003"), Quantity = 50m }),
                    CreateEntity<WorkshopInventory>("03D4E5F6-A7B8-9012-CDEF-123456789012", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"), MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), Quantity = 75m })
                };
                await context.WorkshopInventory.AddRangeAsync(winvs);
            }
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // HELPER GENERIC (CÓ AUTO-CONVERT DATEONLY <-> DATETIME)
        // ==========================================
        private static T CreateEntity<T>(string id, object data) where T : class
        {
            var entity = Activator.CreateInstance(typeof(T), true) as T;
            SetProperty(entity, "Id", Guid.Parse(id));

            foreach (var prop in data.GetType().GetProperties())
            {
                SetProperty(entity, prop.Name, prop.GetValue(data));
            }

            return entity;
        }

        private static void SetProperty(object target, string propName, object value)
        {
            var type = target.GetType();
            var propInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            // 1. Ưu tiên dùng Setter (nếu có)
            if (propInfo != null)
            {
                if (value != null)
                {
                    var targetType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                    var valueType = value.GetType();

                    // Logic chuyển đổi thông minh
                    // A. Target là DateOnly, Value là DateTime
                    if (targetType == typeof(DateOnly) && value is DateTime dt)
                    {
                        value = DateOnly.FromDateTime(dt);
                    }
                    // B. Target là DateTime, Value là DateOnly (FIX LỖI CỦA BẠN)
                    else if (targetType == typeof(DateTime) && value is DateOnly dOnly)
                    {
                        value = dOnly.ToDateTime(TimeOnly.MinValue);
                    }
                    // C. Int -> Decimal
                    else if (targetType != valueType)
                    {
                        try { value = Convert.ChangeType(value, targetType); } catch { }
                    }
                }

                if (propInfo.CanWrite)
                {
                    propInfo.SetValue(target, value);
                    return;
                }
            }

            // 2. Dùng Backing Field (nếu property read-only)
            var field = GetBackingField(type, propName);
            if (field != null)
            {
                if (value != null)
                {
                    var targetType = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;

                    // Logic chuyển đổi tương tự cho field
                    if (targetType == typeof(DateOnly) && value is DateTime dt)
                    {
                        value = DateOnly.FromDateTime(dt);
                    }
                    else if (targetType == typeof(DateTime) && value is DateOnly dOnly)
                    {
                        value = dOnly.ToDateTime(TimeOnly.MinValue);
                    }
                    else if (targetType != value.GetType())
                    {
                        try { value = Convert.ChangeType(value, targetType); } catch { }
                    }
                }
                field.SetValue(target, value);
            }
        }

        private static FieldInfo GetBackingField(Type type, string propName)
        {
            while (type != null)
            {
                var field = type.GetField($"<{propName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null) return field;
                type = type.BaseType;
            }
            return null;
        }
    }
}