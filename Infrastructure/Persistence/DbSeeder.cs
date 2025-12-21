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
            // Xóa dữ liệu cũ (nếu cần thiết, hoặc logic này để ensure idempotent)
            // Trong EF Core Seed, ta thường kiểm tra Any() trước.

            await SeedWorkshopsAsync(context);
            await SeedUsersAsync(context);
            await SeedMaterialsAsync(context);
            await SeedProductsAsync(context);
            await SeedInventoriesAsync(context);
            await SeedBatchesAsync(context);
            await SeedAssignmentsAsync(context);
            await SeedMaterialRequestsAsync(context);
            await SeedMaterialUseAsync(context);
            await SeedMaterialWorkshopsAsync(context);
            await SeedProductionsAsync(context);
            await SeedEvaluatesAndDefectsAsync(context);
            await SeedIncomesAsync(context);
            await SeedNotificationsAsync(context);
            await SeedTransferRequestsAsync(context); // AssignmentTransfer & TaskTransfer
            await SeedWorkshopInventoryAsync(context);

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
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000001", new { Name = "Cắt laser (nếu có)", Description = "Công đoạn Cắt laser (nếu có)", StepOrder = 1, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000002", new { Name = "Cắt vải ép keo", Description = "Công đoạn Cắt vải ép keo", StepOrder = 2, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000003", new { Name = "Dán vải", Description = "Công đoạn Dán vải", StepOrder = 3, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000004", new { Name = "Cắt vải ra đủ bộ", Description = "Công đoạn Cắt vải ra đủ bộ", StepOrder = 4, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000005", new { Name = "In + thêu", Description = "Công đoạn In + thêu", StepOrder = 5, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000006", new { Name = "Làm chỏm", Description = "Công đoạn Làm chỏm", StepOrder = 6, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000007", new { Name = "Làm kết", Description = "Công đoạn Làm kết", StepOrder = 7, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000008", new { Name = "Đóng nút bấm đuôi (nếu có)", Description = "Công đoạn Đóng nút bấm đuôi (nếu có)", StepOrder = 8, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000009", new { Name = "Xỏ cây dựng + vắt sổ", Description = "Công đoạn Xỏ cây dựng + vắt sổ", StepOrder = 9, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000010", new { Name = "Vào nón + vào đai", Description = "Công đoạn Vào nón + vào đai", StepOrder = 10, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000011", new { Name = "Trần đầu nón", Description = "Công đoạn Trần đầu nón", StepOrder = 11, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000012", new { Name = "May đuôi khóa vào nón (nếu có)", Description = "Công đoạn May đuôi khóa vào nón (nếu có)", StepOrder = 12, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000013", new { Name = "May tem", Description = "Công đoạn May tem", StepOrder = 13, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000014", new { Name = "Cắt chỉ dư + xỏ tem thẻ bài", Description = "Công đoạn Cắt chỉ dư + xỏ tem thẻ bài", StepOrder = 14, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000015", new { Name = "Ủi nón", Description = "Công đoạn Ủi nón", StepOrder = 15, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000016", new { Name = "Gấp cây + đóng bịch", Description = "Công đoạn Gấp cây + đóng bịch", StepOrder = 16, WorkshopType = 1, Status = "Assigned", CreatedAt = DateTime.Now }),
                CreateEntity<Workshop>("A1C9B3A0-4F12-4E81-B17B-000000000017", new { Name = "Xưởng khoán", Description = "Xưởng làm tất cả", WorkshopType = 2, Status = "Assigned", CreatedAt = DateTime.Now })
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

            // Hash password "123" (từ SQL: $2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC)
            // Hoặc dùng hasher mặc định để đảm bảo tương thích logic đăng nhập
            var ph = new PasswordHasher();
            var pass = ph.Hash("123");

            var users = new List<User>();

            User U(string id, string wsId, string role, string name, string email, string phone, bool isQcTransport = false)
            {
                var u = User.Create(string.IsNullOrEmpty(wsId) ? Guid.Empty : Guid.Parse(wsId), role, name, email, pass, phone);
                SetProperty(u, "Status", "Active");
                SetProperty(u, "IsQcTransport", isQcTransport);
                SetProperty(u, "CreatedAt", DateTime.Now);
                SetProperty(u, "Id", Guid.Parse(id));
                return u;
            }

            // Core Users
            users.Add(U("A0000000-0000-0000-0000-000000000001", "A1C9B3A0-4F12-4E81-B17B-000000000001", "Admin", "Nguyễn Văn An", "admin@tcaps.com", "0901234567"));
            users.Add(U("A0000000-0000-0000-0000-000000000002", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Lead", "Trần Thị Bình", "lead@tcaps.com", "0902345678"));
            users.Add(U("A0000000-0000-0000-0000-000000000003", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Lê Văn Cường", "staff.cuong@tcaps.com", "0903456789"));
            users.Add(U("A0000000-0000-0000-0000-000000000004", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Phạm Thị Dung", "staff.dung@tcaps.com", "0904567890"));
            users.Add(U("A0000000-0000-0000-0000-000000000005", "A1C9B3A0-4F12-4E81-B17B-000000000003", "QCTransport", "Nguyễn Thị Hạnh", "qctransport@tcaps.com", "0906789012", true));

            // QCs (Users 6-21 + 56)
            users.Add(U("A0000000-0000-0000-0000-000000000006", "A1C9B3A0-4F12-4E81-B17B-000000000001", "QC", "QC Xưởng Cắt Laser", "qc.laser@tcaps.com", "0901000101"));
            users.Add(U("A0000000-0000-0000-0000-000000000007", "A1C9B3A0-4F12-4E81-B17B-000000000002", "QC", "Hoàng Văn Em", "qc.catkeo@tcaps.com", "0901000102"));
            users.Add(U("A0000000-0000-0000-0000-000000000008", "A1C9B3A0-4F12-4E81-B17B-000000000003", "QC", "Khánh Nguyệt", "qc.danvai@tcaps.com", "0901000103"));
            users.Add(U("A0000000-0000-0000-0000-000000000009", "A1C9B3A0-4F12-4E81-B17B-000000000004", "QC", "QC Xưởng Cắt Đủ Bộ", "qc.catdubo@tcaps.com", "0901000104"));
            users.Add(U("A0000000-0000-0000-0000-000000000010", "A1C9B3A0-4F12-4E81-B17B-000000000005", "QC", "Anh Dũng", "qc.intheu@tcaps.com", "0901000105"));
            users.Add(U("A0000000-0000-0000-0000-000000000011", "A1C9B3A0-4F12-4E81-B17B-000000000006", "QC", "QC Xưởng Làm Chỏm", "qc.lamchom@tcaps.com", "0901000106"));
            users.Add(U("A0000000-0000-0000-0000-000000000012", "A1C9B3A0-4F12-4E81-B17B-000000000007", "QC", "QC Xưởng Làm Kết", "qc.lamket@tcaps.com", "0901000107"));
            users.Add(U("A0000000-0000-0000-0000-000000000013", "A1C9B3A0-4F12-4E81-B17B-000000000008", "QC", "QC Xưởng Đóng Nút", "qc.dongnut@tcaps.com", "0901000108"));
            users.Add(U("A0000000-0000-0000-0000-000000000014", "A1C9B3A0-4F12-4E81-B17B-000000000009", "QC", "QC Xưởng Vắt Sổ", "qc.vatso@tcaps.com", "0901000109"));
            users.Add(U("A0000000-0000-0000-0000-000000000015", "A1C9B3A0-4F12-4E81-B17B-000000000010", "QC", "QC Xưởng Vào Đai", "qc.vaodai@tcaps.com", "0901000110"));
            users.Add(U("A0000000-0000-0000-0000-000000000016", "A1C9B3A0-4F12-4E81-B17B-000000000011", "QC", "QC Xưởng Trần Đầu", "qc.trandau@tcaps.com", "0901000111"));
            users.Add(U("A0000000-0000-0000-0000-000000000017", "A1C9B3A0-4F12-4E81-B17B-000000000012", "QC", "QC Xưởng May Khóa", "qc.maykhoa@tcaps.com", "0901000112"));
            users.Add(U("A0000000-0000-0000-0000-000000000018", "A1C9B3A0-4F12-4E81-B17B-000000000013", "QC", "QC Xưởng May Tem", "qc.maytem@tcaps.com", "0901000113"));
            users.Add(U("A0000000-0000-0000-0000-000000000019", "A1C9B3A0-4F12-4E81-B17B-000000000014", "QC", "QC Xưởng Cắt Chỉ", "qc.catchi@tcaps.com", "0901000114"));
            users.Add(U("A0000000-0000-0000-0000-000000000020", "A1C9B3A0-4F12-4E81-B17B-000000000015", "QC", "QC Xưởng Ủi Nón", "qc.uinon@tcaps.com", "0901000115"));
            users.Add(U("A0000000-0000-0000-0000-000000000021", "A1C9B3A0-4F12-4E81-B17B-000000000016", "QC", "QC Xưởng Đóng Bịch", "qc.dongbich@tcaps.com", "0901000116"));
            users.Add(U("A0000000-0000-0000-0000-000000000056", "A1C9B3A0-4F12-4E81-B17B-000000000017", "QC", "QC Xưởng Khoán", "qc.khoan@tcaps.com", "0901000116"));

            // Staffs (22-55) - Chi tiết theo từng xưởng
            users.Add(U("A0000000-0000-0000-0000-000000000022", "A1C9B3A0-4F12-4E81-B17B-000000000001", "Staff", "Nguyễn Văn A - Laser", "staff.laser1@tcaps.com", "0902010101"));
            users.Add(U("A0000000-0000-0000-0000-000000000023", "A1C9B3A0-4F12-4E81-B17B-000000000001", "Staff", "Trần Thị B - Laser", "staff.laser2@tcaps.com", "0902010102"));
            users.Add(U("A0000000-0000-0000-0000-000000000024", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Lê Văn C - Cắt Keo", "staff.catkeo1@tcaps.com", "0902020201"));
            users.Add(U("A0000000-0000-0000-0000-000000000025", "A1C9B3A0-4F12-4E81-B17B-000000000002", "Staff", "Phạm Thị D - Cắt Keo", "staff.catkeo2@tcaps.com", "0902020202"));
            users.Add(U("A0000000-0000-0000-0000-000000000026", "A1C9B3A0-4F12-4E81-B17B-000000000003", "Staff", "Hoàng Văn E - Dán Vải", "staff.danvai1@tcaps.com", "0902030301"));
            users.Add(U("A0000000-0000-0000-0000-000000000027", "A1C9B3A0-4F12-4E81-B17B-000000000003", "Staff", "Nguyễn Thị F - Dán Vải", "staff.danvai2@tcaps.com", "0902030302"));
            users.Add(U("A0000000-0000-0000-0000-000000000028", "A1C9B3A0-4F12-4E81-B17B-000000000004", "Staff", "Phan Văn G - Cắt Đủ Bộ", "staff.catdubo1@tcaps.com", "0902040401"));
            users.Add(U("A0000000-0000-0000-0000-000000000029", "A1C9B3A0-4F12-4E81-B17B-000000000004", "Staff", "Vũ Thị H - Cắt Đủ Bộ", "staff.catdubo2@tcaps.com", "0902040402"));
            users.Add(U("A0000000-0000-0000-0000-000000000030", "A1C9B3A0-4F12-4E81-B17B-000000000005", "Staff", "Đỗ Văn I - In Thêu", "staff.intheu1@tcaps.com", "0902050501"));
            users.Add(U("A0000000-0000-0000-0000-000000000031", "A1C9B3A0-4F12-4E81-B17B-000000000005", "Staff", "Trương Thị K - In Thêu", "staff.intheu2@tcaps.com", "0902050502"));
            users.Add(U("A0000000-0000-0000-0000-000000000032", "A1C9B3A0-4F12-4E81-B17B-000000000006", "Staff", "Nguyễn Văn L - Làm Chỏm", "staff.lamchom1@tcaps.com", "0902060601"));
            users.Add(U("A0000000-0000-0000-0000-000000000033", "A1C9B3A0-4F12-4E81-B17B-000000000006", "Staff", "Trần Thị M - Làm Chỏm", "staff.lamchom2@tcaps.com", "0902060602"));
            users.Add(U("A0000000-0000-0000-0000-000000000034", "A1C9B3A0-4F12-4E81-B17B-000000000007", "Staff", "Lê Văn N - Làm Kết", "staff.lamket1@tcaps.com", "0902070701"));
            users.Add(U("A0000000-0000-0000-0000-000000000035", "A1C9B3A0-4F12-4E81-B17B-000000000007", "Staff", "Phạm Thị O - Làm Kết", "staff.lamket1@tcaps.com", "0902070702"));
            users.Add(U("A0000000-0000-0000-0000-000000000036", "A1C9B3A0-4F12-4E81-B17B-000000000008", "Staff", "Hoàng Văn P - Đóng Nút", "staff.dongnut1@tcaps.com", "0902080801"));
            users.Add(U("A0000000-0000-0000-0000-000000000037", "A1C9B3A0-4F12-4E81-B17B-000000000008", "Staff", "Nguyễn Thị Q - Đóng Nút", "staff.dongnut2@tcaps.com", "0902080802"));
            users.Add(U("A0000000-0000-0000-0000-000000000038", "A1C9B3A0-4F12-4E81-B17B-000000000009", "Staff", "Phan Văn R - Vắt Sổ", "staff.vatso1@tcaps.com", "0902090901"));
            users.Add(U("A0000000-0000-0000-0000-000000000039", "A1C9B3A0-4F12-4E81-B17B-000000000009", "Staff", "Vũ Thị S - Vắt Sổ", "staff.vatso2@tcaps.com", "0902090902"));
            users.Add(U("A0000000-0000-0000-0000-000000000040", "A1C9B3A0-4F12-4E81-B17B-000000000010", "Staff", "Đỗ Văn T - Vào Đai", "staff.vaodai1@tcaps.com", "0902101001"));
            users.Add(U("A0000000-0000-0000-0000-000000000041", "A1C9B3A0-4F12-4E81-B17B-000000000010", "Staff", "Trương Thị U - Vào Đai", "staff.vaodai2@tcaps.com", "0902101002"));
            users.Add(U("A0000000-0000-0000-0000-000000000042", "A1C9B3A0-4F12-4E81-B17B-000000000011", "Staff", "Nguyễn Văn V - Trần Đầu", "staff.trandau1@tcaps.com", "0902111101"));
            users.Add(U("A0000000-0000-0000-0000-000000000043", "A1C9B3A0-4F12-4E81-B17B-000000000011", "Staff", "Trần Thị X - Trần Đầu", "staff.trandau2@tcaps.com", "0902111102"));
            users.Add(U("A0000000-0000-0000-0000-000000000044", "A1C9B3A0-4F12-4E81-B17B-000000000012", "Staff", "Lê Văn Y - May Khóa", "staff.maykhoa1@tcaps.com", "0902121201"));
            users.Add(U("A0000000-0000-0000-0000-000000000045", "A1C9B3A0-4F12-4E81-B17B-000000000012", "Staff", "Phạm Thị Z - May Khóa", "staff.maykhoa2@tcaps.com", "0902121202"));
            users.Add(U("A0000000-0000-0000-0000-000000000046", "A1C9B3A0-4F12-4E81-B17B-000000000013", "Staff", "Hoàng Văn AA - May Tem", "staff.maytem1@tcaps.com", "0902131301"));
            users.Add(U("A0000000-0000-0000-0000-000000000047", "A1C9B3A0-4F12-4E81-B17B-000000000013", "Staff", "Nguyễn Thị BB - May Tem", "staff.maytem2@tcaps.com", "0902131302"));
            users.Add(U("A0000000-0000-0000-0000-000000000048", "A1C9B3A0-4F12-4E81-B17B-000000000014", "Staff", "Phan Văn CC - Cắt Chỉ", "staff.catchi1@tcaps.com", "0902141401"));
            users.Add(U("A0000000-0000-0000-0000-000000000049", "A1C9B3A0-4F12-4E81-B17B-000000000014", "Staff", "Vũ Thị DD - Cắt Chỉ", "staff.catchi2@tcaps.com", "0902141402"));
            users.Add(U("A0000000-0000-0000-0000-000000000050", "A1C9B3A0-4F12-4E81-B17B-000000000015", "Staff", "Đỗ Văn EE - Ủi Nón", "staff.uinon1@tcaps.com", "0902151501"));
            users.Add(U("A0000000-0000-0000-0000-000000000051", "A1C9B3A0-4F12-4E81-B17B-000000000015", "Staff", "Trương Thị FF - Ủi Nón", "staff.uinon2@tcaps.com", "0902151502"));
            users.Add(U("A0000000-0000-0000-0000-000000000052", "A1C9B3A0-4F12-4E81-B17B-000000000016", "Staff", "Nguyễn Văn GG - Đóng Bịch", "staff.dongbich1@tcaps.com", "0902161601"));
            users.Add(U("A0000000-0000-0000-0000-000000000053", "A1C9B3A0-4F12-4E81-B17B-000000000016", "Staff", "Trần Thị HH - Đóng Bịch", "staff.dongbich2@tcaps.com", "0902161602"));

            // Xưởng khoán
            users.Add(U("A0000000-0000-0000-0000-000000000054", "A1C9B3A0-4F12-4E81-B17B-000000000017", "Staff", "Nguyễn Văn GG - Khoán", "staff.khoan1@tcaps.com", "0902161601"));
            users.Add(U("A0000000-0000-0000-0000-000000000055", "A1C9B3A0-4F12-4E81-B17B-000000000017", "Staff", "Trần Thị HH - Khoán", "staff.khoan2@tcaps.com", "0902161602"));

            // Other
            users.Add(U("A0000000-0000-0000-0000-000000000057", "A1C9B3A0-4F12-4E81-B17B-000000000017", "GuardQC", "QC Gác Cổng", "qc.gaccong@tcaps.com", "0902161602"));
            users.Add(U("A0000000-0000-0000-0000-000000000058", "A1C9B3A0-4F12-4E81-B17B-000000000017", "Lead", "Lead1", "lead1@tcaps.com", "0902161602"));
            users.Add(U("A0000000-0000-0000-0000-000000000059", "A1C9B3A0-4F12-4E81-B17B-000000000017", "Lead", "Lead2", "lead2@tcaps.com", "0902161602"));

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
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000001", new { Code = "NON-001", Name = "Nón bảo hiểm 3/4 đầu", Image = "/images/products/helmet_34.jpg", Description = "Nón bảo hiểm 3/4 đầu có kính, chất liệu ABS cao cấp", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000002", new { Code = "NON-002", Name = "Nón bảo hiểm fullface", Image = "/images/products/helmet_fullface.jpg", Description = "Nón bảo hiểm fullface nguyên đầu, tiêu chuẩn DOT", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000003", new { Code = "NON-003", Name = "Nón bảo hiểm nửa đầu", Image = "/images/products/helmet_half.jpg", Description = "Nón bảo hiểm nửa đầu thời trang, nhẹ và thoáng khí", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000004", new { Code = "NON-004", Name = "Nón bảo hiểm trẻ em", Image = "/images/products/helmet_kids.jpg", Description = "Nón bảo hiểm dành cho trẻ em, nhiều màu sắc", IsDeleted = false, CreatedAt = DateTime.Now }),
                CreateEntity<Product>("C0000000-0000-0000-0000-000000000005", new { Code = "NON-005", Name = "Nón bảo hiểm thể thao", Image = "/images/products/helmet_sport.jpg", Description = "Nón bảo hiểm thể thao Motocross, chất liệu composite", IsDeleted = false, CreatedAt = DateTime.Now })
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 5. BATCHES
        // ==========================================
        private static async Task SeedBatchesAsync(AppDbContext context)
        {
            if (await context.Batches.AnyAsync()) return;
            var batches = new List<Batch>
            {
                CreateEntity<Batch>("D0000000-0000-0000-0000-000000000001", new { ProductId = Guid.Parse("C0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000002"), Code = "BATCH-2025-001", Quantity = 100m, ActualQuantity = 100m, LostQuantity = 0m, StartDate = DateTime.Parse("2025-10-01"), EndDate = DateTime.Parse("2025-11-12"), Status = "Completed", IsDeleted = false, CreatedAt = DateTime.Now })
            };
            await context.Batches.AddRangeAsync(batches);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 6. ASSIGNMENTS
        // ==========================================
        private static async Task SeedAssignmentsAsync(AppDbContext context)
        {
            if (await context.Assignments.AnyAsync()) return;
            var assigns = new List<Assignment>
            {
                CreateEntity<Assignment>("E0000000-0000-0000-0000-000000000001", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), StepOrder = 1, Quantity = 100m, UnitPrice = 10000m, StartDate = DateTime.Parse("2025-11-01"), EndDate = DateTime.Parse("2025-11-03"), ExpectedDeliveryDate = DateTime.Parse("2025-11-03"), RequiresMaterialDelivery = true, Status = "Completed", CreatedAt = DateTime.Now })
            };
            await context.Assignments.AddRangeAsync(assigns);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 7. MATERIAL REQUESTS
        // ==========================================
        private static async Task SeedMaterialRequestsAsync(AppDbContext context)
        {
            if (await context.MaterialRequests.AnyAsync()) return;
            var reqs = new List<MaterialRequest>
            {
                CreateEntity<MaterialRequest>("F0000000-0000-0000-0000-000000000001", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000008"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), QuantityRequest = 200m, Status = "Confirmed", Note = "Cần gấp cho công đoạn cắt laser", Date = DateTime.Parse("2025-11-01"), Type = "LeadExport", ActualReceivedQuantity = 0m, QuantityFromStock = 0m })
            };
            await context.MaterialRequests.AddRangeAsync(reqs);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 8. INVENTORIES
        // ==========================================
        private static async Task SeedInventoriesAsync(AppDbContext context)
        {
            if (await context.Inventories.AnyAsync()) return;
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
        // 9. MATERIAL USE
        // ==========================================
        private static async Task SeedMaterialUseAsync(AppDbContext context)
        {
            if (await context.MaterialUse.AnyAsync()) return;
            var uses = new List<MaterialUse>
            {
                CreateEntity<MaterialUse>("20000000-0000-0000-0000-000000000001", new { MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), QuantityDivide = 200m, QuantityStaffUse = 0m, ReconciledQuantity = 0m, QuantityRequest = 0m, Date = DateTime.Parse("2025-11-02") })
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
                CreateEntity<MaterialWorkshop>("30000000-0000-0000-0000-000000000001", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), AssignmentTransferRequestId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-56789abcdef0"), SupplierId = Guid.Parse("A0000000-0000-0000-0000-000000000002"), QuantitySend = 100m, QuantityReceive = 0m, ShipDate = DateTime.Parse("2025-11-01"), Status = "Confirmed", CreatedAt = DateTime.Now })
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
                CreateEntity<Production>("40000000-0000-0000-0000-000000000001", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000026"), Quantity = 50m, Date = DateTime.Parse("2025-11-03"), Time = TimeSpan.Parse("08:30:00"), Status = "Passed" }),
                CreateEntity<Production>("40000000-0000-0000-0000-000000000002", new { AssignId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000027"), Quantity = 50m, Date = DateTime.Parse("2025-11-04"), Time = TimeSpan.Parse("08:30:00"), Status = "Passed" })
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
                CreateEntity<Evaluate>("50000000-0000-0000-0000-000000000001", new { ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000008"), Status = "Passed", Note = "Chất lượng tốt, không có lỗi", QuantityError = 0m, QuantitySuccess = 100m, Image = "/images/qc/qc_001.jpg", CreatedAt = DateTime.Now }),
                CreateEntity<Evaluate>("50000000-0000-0000-0000-000000000002", new { ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000008"), Status = "CompleteWithLoss", Note = "Fullface đạt tiêu chuẩn", QuantityError = 5m, QuantitySuccess = 50m, Image = "/images/qc/qc_002.jpg", CreatedAt = DateTime.Now })
            };
            await context.Evaluates.AddRangeAsync(evas);
            await context.SaveChangesAsync(CancellationToken.None);

            if (await context.ComponentDefects.AnyAsync()) return;
            var defects = new List<ComponentDefect>
            {
                CreateEntity<ComponentDefect>("60000000-0000-0000-0000-000000000001", new { EvaluateId = Guid.Parse("50000000-0000-0000-0000-000000000002"), Description = "Vết xước nhỏ trên bề mặt nón", Quantity = 3, CreatedAt = DateTime.Now, Status = "Unfixabled" }),
                CreateEntity<ComponentDefect>("60000000-0000-0000-0000-000000000002", new { EvaluateId = Guid.Parse("50000000-0000-0000-0000-000000000002"), Description = "Khóa cài có độ lỏng nhẹ", Quantity = 2, CreatedAt = DateTime.Now, Status = "Confirmed" })
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
                CreateEntity<Income>("70000000-0000-0000-0000-000000000001", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000026"), Quantity = 50m, TotalPrice = 500000m, CreatedAt = DateTime.Now }),
                CreateEntity<Income>("70000000-0000-0000-0000-000000000002", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), ProductionId = Guid.Parse("40000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000027"), Quantity = 48m, TotalPrice = 480000m, CreatedAt = DateTime.Now })
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
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000001", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Title = "Chào mừng", Message = "Chào mừng bạn đến với hệ thống sản xuất", Type = "System", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000002", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Title = "Nhiệm vụ mới", Message = "Bạn có nhiệm vụ sản xuất lô BATCH-2025-001", Type = "Task", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000003", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000004"), Title = "Nhiệm vụ mới", Message = "Bạn có nhiệm vụ sản xuất lô BATCH-2025-002", Type = "Task", IsRead = false, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000004", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), Title = "Yêu cầu kiểm tra", Message = "Có sản phẩm mới cần kiểm tra chất lượng", Type = "QC", IsRead = true, CreatedAt = DateTime.Now }),
                CreateEntity<Notification>("80000000-0000-0000-0000-000000000005", new { UserId = Guid.Parse("A0000000-0000-0000-0000-000000000002"), Title = "Báo cáo tuần", Message = "Vui lòng nộp báo cáo sản xuất tuần này", Type = "Report", IsRead = true, CreatedAt = DateTime.Now })
            };
            await context.Notifications.AddRangeAsync(notis);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 15. TRANSFER REQUESTS (Assignment & Task)
        // ==========================================
        private static async Task SeedTransferRequestsAsync(AppDbContext context)
        {
            // AssignmentTransferRequests
            if (!await context.AssignmentTransferRequests.AnyAsync())
            {
                var atrs = new List<AssignmentTransferRequest>
                {
                    CreateEntity<AssignmentTransferRequest>("a1b2c3d4-e5f6-7890-1234-56789abcdef0", new { AssignmentId = Guid.Parse("E0000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), CompletedQuantitySend = 98m, CompletedQuantityReceive = 0m, Status = "PendingApproval", Note = "Yêu cầu chuyển giao lần 1", CreatedAt = DateTime.Now })
                };
                await context.AssignmentTransferRequests.AddRangeAsync(atrs);
            }

            // TaskTransferRequests
            if (!await context.TaskTransferRequests.AnyAsync())
            {
                var ttrs = new List<TaskTransferRequest>
                {
                    CreateEntity<TaskTransferRequest>("A3F2504E-4F89-11D3-9A0C-0305E82C3301", new { BatchId = Guid.Parse("D0000000-0000-0000-0000-000000000001"), WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), QcTransportId = Guid.Parse("A0000000-0000-0000-0000-000000000005"), MaterialRequestId = Guid.Parse("F0000000-0000-0000-0000-000000000001"), Status = "Pending", Note = "Test insert 1", CreatedAt = DateTime.Now })
                };
                await context.TaskTransferRequests.AddRangeAsync(ttrs);
            }
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // ==========================================
        // 16. WORKSHOP INVENTORY
        // ==========================================
        private static async Task SeedWorkshopInventoryAsync(AppDbContext context)
        {
            if (await context.WorkshopInventory.AnyAsync()) return;
            var winvs = new List<WorkshopInventory>
            {
                CreateEntity<WorkshopInventory>("01B2C3D4-E5F6-7890-ABCD-EF1234567890", new { WorkshopId = Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"), MaterialId = Guid.Parse("B0000000-0000-0000-0000-000000000001"), Quantity = 100m, HoldingQuantity = 0m })
            };
            await context.WorkshopInventory.AddRangeAsync(winvs);
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

                    // A. DateOnly <-> DateTime
                    if (targetType == typeof(DateOnly) && value is DateTime dt)
                    {
                        value = DateOnly.FromDateTime(dt);
                    }
                    else if (targetType == typeof(DateTime) && value is DateOnly dOnly)
                    {
                        value = dOnly.ToDateTime(TimeOnly.MinValue);
                    }
                    // B. TimeOnly <-> TimeSpan (FIX LỖI CỦA BẠN Ở ĐÂY)
                    else if (targetType == typeof(TimeOnly) && value is TimeSpan ts)
                    {
                        value = TimeOnly.FromTimeSpan(ts);
                    }
                    else if (targetType == typeof(TimeSpan) && value is TimeOnly tOnly)
                    {
                        value = tOnly.ToTimeSpan();
                    }
                    // C. Các loại khác
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
                    // B. TimeOnly <-> TimeSpan (FIX LỖI CỦA BẠN Ở ĐÂY)
                    else if (targetType == typeof(TimeOnly) && value is TimeSpan ts)
                    {
                        value = TimeOnly.FromTimeSpan(ts);
                    }
                    else if (targetType == typeof(TimeSpan) && value is TimeOnly tOnly)
                    {
                        value = tOnly.ToTimeSpan();
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