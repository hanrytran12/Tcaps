-- INSERT dữ liệu cho bảng User
select * from Users
INSERT INTO [Users] 
    (Id, WorkshopId, Role, FullName, Email, PasswordHash, Phone, Status, IsQcTransport, CreatedAt)
VALUES 
('A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'F6A7B8C9-D1E2-4F5A-4B5C-6D7E8F9A1B2C', 'Admin', N'Nguyễn Văn An', 'admin@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0901234567', 'Active', 0, GETDATE()),
('B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'F6A7B8C9-D1E2-4F5A-4B5C-6D7E8F9A1B2C', 'Lead', N'Trần Thị Bình', 'manager@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0902345678', 'Active', 0, GETDATE()),
('C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Lê Văn Cường', 'worker1@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0903456789', 'Active', 0, GETDATE()),
('D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Phạm Thị Dung', 'worker2@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0904567890', 'Active', 0, GETDATE()),
('E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QC', N'Hoàng Văn Em', 'qc@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0905678901', 'Active', 0, GETDATE()),
('F6A7B8C9-D1E2-4F5A-8B6C-1D2E3F4A5B6C', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QCTransport', N'Nguyễn Thị Hạnh', 'qctransport@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0906789012', 'Active', 1, GETDATE());

-- INSERT dữ liệu cho bảng Workshop
INSERT INTO Workshop (Id, Name, Description, StepOrder)
VALUES 
-- 1. Cắt laser (nếu có)
('A1C9B3A0-4F12-4E81-B17B-000000000003', N'Cắt laser (nếu có)', N'Công đoạn Cắt laser (nếu có)', 1),
-- 2. Cắt vải ép keo
('A1C9B3A0-4F12-4E81-B17B-000000000004', N'Cắt vải ép keo', N'Công đoạn Cắt vải ép keo', 2),
-- 3. Dán vải
('A1C9B3A0-4F12-4E81-B17B-000000000005', N'Dán vải', N'Công đoạn Dán vải', 3),
-- 4. Cắt vải ra đủ bộ
('A1C9B3A0-4F12-4E81-B17B-000000000006', N'Cắt vải ra đủ bộ', N'Công đoạn Cắt vải ra đủ bộ', 4),
-- 5. In + thêu
('A1C9B3A0-4F12-4E81-B17B-000000000007', N'In + thêu', N'Công đoạn In + thêu', 5),
-- 6. Làm chỏm
('A1C9B3A0-4F12-4E81-B17B-000000000008', N'Làm chỏm', N'Công đoạn Làm chỏm', 6),
-- 7. Làm kết
('A1C9B3A0-4F12-4E81-B17B-000000000009', N'Làm kết', N'Công đoạn Làm kết', 7),
-- 8. Đóng nút bấm đuôi (nếu có)
('A1C9B3A0-4F12-4E81-B17B-000000000010', N'Đóng nút bấm đuôi (nếu có)', N'Công đoạn Đóng nút bấm đuôi (nếu có)', 8),
-- 9. Xỏ cây dựng + vắt sổ
('A1C9B3A0-4F12-4E81-B17B-000000000011', N'Xỏ cây dựng + vắt sổ', N'Công đoạn Xỏ cây dựng + vắt sổ', 9),
-- 10. Vào nón + vào đai
('A1C9B3A0-4F12-4E81-B17B-000000000012', N'Vào nón + vào đai', N'Công đoạn Vào nón + vào đai', 10),
-- 11. Trần đầu nón
('A1C9B3A0-4F12-4E81-B17B-000000000013', N'Trần đầu nón', N'Công đoạn Trần đầu nón', 11),
-- 12. May đuôi khóa vào nón (nếu có)
('A1C9B3A0-4F12-4E81-B17B-000000000014', N'May đuôi khóa vào nón (nếu có)', N'Công đoạn May đuôi khóa vào nón (nếu có)', 12),
-- 13. May tem
('A1C9B3A0-4F12-4E81-B17B-000000000015', N'May tem', N'Công đoạn May tem', 13),
-- 14. Cắt chỉ dư + xỏ tem thẻ bài
('A1C9B3A0-4F12-4E81-B17B-000000000016', N'Cắt chỉ dư + xỏ tem thẻ bài', N'Công đoạn Cắt chỉ dư + xỏ tem thẻ bài', 14),
-- 15. Ủi nón
('A1C9B3A0-4F12-4E81-B17B-000000000017', N'Ủi nón', N'Công đoạn Ủi nón', 15),
-- 16. Gấp cây + đóng bịch
('A1C9B3A0-4F12-4E81-B17B-000000000018', N'Gấp cây + đóng bịch', N'Công đoạn Gấp cây + đóng bịch', 16);

-- INSERT dữ liệu cho bảng Notifications
INSERT INTO [Notifications] (Id, UserId, Title, Message, Type, IsRead, CreatedAt)
VALUES 
('C9D1E2F3-A4B5-4C5D-7E8F-9A1B2C3D4E5F', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', N'Chào mừng', N'Chào mừng bạn đến với hệ thống sản xuất', N'System', 0, GETDATE()),
('D1E2F3A4-B5C6-4D5E-8F9A-1B2C3D4E5F6A', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', N'Nhiệm vụ mới', N'Bạn có nhiệm vụ sản xuất lô BATCH-2024-001', N'Task', 0, GETDATE()),
('E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', N'Nhiệm vụ mới', N'Bạn có nhiệm vụ lắp ráp tại xưởng lắp ráp', N'Task', 0, GETDATE()),
('F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', N'Yêu cầu kiểm tra', N'Có sản phẩm mới cần kiểm tra chất lượng', N'QC', 1, GETDATE()),
('A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 'B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', N'Báo cáo tuần', N'Vui lòng nộp báo cáo sản xuất tuần này', N'Report', 1, GETDATE());

-- INSERT dữ liệu cho bảng Materials
INSERT INTO [Materials] (Id, Name, Description, Quantity, Price, Unit)
VALUES 
('F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', N'Thép tấm A36', N'Thép tấm carbon thường dùng trong công nghiệp', 1000, 15000, N'Kg'),
('A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', N'Ốc vít M8', N'Ốc vít đường kính 8mm tiêu chuẩn', 5000, 500, N'Cái'),
('B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', N'Sơn chống gỉ', N'Sơn chống gỉ màu xám công nghiệp', 200, 80000, N'Lít'),
('C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', N'Ổ bi 6205', N'Ổ bi đường kính trong 25mm', 500, 25000, N'Cái'),
('D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', N'Dầu thủy lực', N'Dầu thủy lực ISO VG 46', 150, 120000, N'Lít'),
('E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', N'Băng tải cao su', N'Băng tải cao su độ dày 5mm', 300, 250000, N'Mét');
-- INSERT dữ liệu cho bảng Product (Non)
INSERT INTO [Products] (Id, Code, Name, Image, Description, IsDeleted)
VALUES 
('D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'NON-001', N'Nón bảo hiểm 3/4 đầu', N'/images/products/helmet_34.jpg', N'Nón bảo hiểm 3/4 đầu có kính, chất liệu ABS cao cấp', 0),
('E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', 'NON-002', N'Nón bảo hiểm fullface', N'/images/products/helmet_fullface.jpg', N'Nón bảo hiểm fullface nguyên đầu, tiêu chuẩn DOT', 0),
('F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', 'NON-003', N'Nón bảo hiểm nửa đầu', N'/images/products/helmet_half.jpg', N'Nón bảo hiểm nửa đầu thời trang, nhẹ và thoáng khí', 0),
('A1B2C3D4-E5F6-4A5B-8C9D-2E2F2A2B2C2D', 'NON-004', N'Nón bảo hiểm trẻ em', N'/images/products/helmet_kids.jpg', N'Nón bảo hiểm dành cho trẻ em, nhiều màu sắc', 0),
('B2C3D4E5-F6A7-4B5C-9D1E-3F3A3B3C3D3E', 'NON-005', N'Nón bảo hiểm thể thao', N'/images/products/helmet_sport.jpg', N'Nón bảo hiểm thể thao Motocross, chất liệu composite', 0);

-- INSERT dữ liệu cho bảng Batches (Lô hàng)
INSERT INTO [dbo].[Batches] 
    ([Id], [ProductId], [Code], [Quantity], [StartDate], [EndDate], [CreatedAt], [Status], [isDeleted])
VALUES 
    ('A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'BATCH-2024-001', 100,  '2024-10-01', '2024-10-15', GETDATE(), N'Đang sản xuất', 0),
    ('B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', 'BATCH-2024-002', 50,  '2024-10-05', '2024-10-20', GETDATE(), N'Đang sản xuất', 0),
    ('C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'BATCH-2024-003', 150,  '2024-10-10', '2024-10-25', GETDATE(), N'Chưa bắt đầu', 0),
    ('D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 'F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', 'BATCH-2024-004', 80,  '2024-10-12', '2024-10-28', GETDATE(), N'Chưa bắt đầu', 0),
    ('E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'A1B2C3D4-E5F6-4A5B-8C9D-2E2F2A2B2C2D', 'BATCH-2024-005', 120,  '2024-09-25', '2024-10-08', GETDATE(), N'Hoàn thành', 0);

-- INSERT dữ liệu cho bảng MaterialRequest
INSERT INTO MaterialRequests (Id, MaterialId, UserId, BatchId, AssignId, QuantityRequest, Status, Note, Date)
VALUES 
('D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 500, N'Đã duyệt', N'Cần gấp', '2024-10-01'),
('E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 1000, N'Đã duyệt', N'Đơn thường', '2024-10-01'),
('F6A7B8C9-D1E2-4F5A-4B5C-6D7E8F9A1B2C', 'B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', 'B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 50, N'Chờ duyệt', N'Cho lô sơn', '2024-10-05');

-- INSERT dữ liệu cho bảng Inventory (Kho)
INSERT INTO [TcapsDB].[dbo].[Inventories] 
    ([Id], [MaterialId], [Quantity], [Date], [Price], [ImageURL])
VALUES 
    ('A7B8C9D1-E2F3-4A5B-5C6D-7E8F9A1B2C3D', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 800,  GETDATE(), 15000, 'https://example.com/images/inventory1.jpg'),
    ('B8C9D1E2-F3A4-4B5C-6D7E-8F9A1B2C3D4E', 'A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 4500, GETDATE(), 8000,  'https://example.com/images/inventory2.jpg'),
    ('C9D1E2F3-A4B5-4C5D-7E8F-9A1B2C3D4E5F', 'B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 180,  GETDATE(), 12000, 'https://example.com/images/inventory3.jpg'),
    ('D1E2F3A4-B5C6-4D5E-8F9A-1B2C3D4E5F6A', 'C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 450,  GETDATE(), 9500,  'https://example.com/images/inventory4.jpg'),
    ('E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 120,  GETDATE(), 30000, 'https://example.com/images/inventory5.jpg'),
    ('F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', 280,  GETDATE(), 22000, 'https://example.com/images/inventory6.jpg');

-- INSERT dữ liệu cho bảng MaterialUse (VL sử dụng theo lô)
INSERT INTO [TcapsDB].[dbo].[MaterialUse] 
(Id, MaterialId, BatchId, AssignId, QuantityDivide, QuantityStaffUse, ReconciledQuantity, QuantityRequest, Date)
VALUES 
('A7B8C9D1-E2F3-4A5B-5C6D-7E8F9A1B2C3D', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 800, 200, 750, 1000, GETDATE()),
('B8C9D1E2-F3A4-4B5C-6D7E-8F9A1B2C3D4E', 'A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 4500, 500, 4400, 5000, GETDATE()),
('C9D1E2F3-A4B5-4C5D-7E8F-9A1B2C3D4E5F', 'B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 'B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 180, 20, 170, 200, GETDATE()),
('D1E2F3A4-B5C6-4D5E-8F9A-1B2C3D4E5F6A', 'C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 'A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 450, 50, 430, 500, GETDATE()),
('E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 'B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 120, 40, 110, 160, GETDATE());

-- INSERT dữ liệu cho bảng Assignments
INSERT INTO [TcapsDB].[dbo].[Assignments] 
    ([Id], [BatchId], [WorkshopId], [StepOrder], [Quantity], [UnitPrice], [StartDate], [EndDate], [ExpectedDeliveryDate], [RequiresMaterialDelivery], [Status], [CreatedAt])
VALUES 
    ('E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 1, 100, 25000, '2024-10-01', '2024-10-10', '2024-10-12', 1, N'Pending', GETDATE()),
    ('F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 2, 50, 30000, '2024-10-05', '2024-10-15', '2024-10-18', 1, N'Pending', GETDATE()),
    ('A4B5C6D7-E8F9-4A5B-2C3D-4E5F6A7B8C9D', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'A1C9B3A0-4F12-4E81-B17B-000000000005', 3, 100, 22000, '2024-10-11', '2024-10-15', '2024-10-17', 1, N'Pending', GETDATE()),
    ('B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 'A1C9B3A0-4F12-4E81-B17B-000000000006', 4, 150, 20000, '2024-10-10', '2024-10-22', '2024-10-25', 1, N'Pending', GETDATE()),
    ('C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 5, 80, 27000, '2024-10-12', '2024-10-25', '2024-10-28', 1, N'Pending', GETDATE());


-- INSERT dữ liệu cho bảng Income
INSERT INTO [Incomes] (Id, BatchId, ProductionId, UserId, Quantity, TotalPrice, CreatedAt)
VALUES 
('B5C6D7E8-F9A1-4B5C-3D4E-5F6A7B8C9D1E', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 50, 25000000, GETDATE()),
('C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 25, 20000000, GETDATE()),
('D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 30, 15000000, GETDATE());

-- INSERT dữ liệu cho bảng Productions (Sản lượng)
INSERT INTO [Productions] (Id, AssignId, UserId, Quantity, Date, Status)
VALUES 
('C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 50, '2024-10-05', N'Đã kiểm tra'),
('D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 25, '2024-10-08', N'Đã kiểm tra'),
('E8F9A1B2-C3D4-4E5F-6A7B-8C9D1E2F3A4B', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 30, '2024-10-09', N'Chờ kiểm tra'),
('F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', 'F3A4B5C6-D7E8-4F5A-1B2C-3D4E5F6A7B8C', 'D4E5F6A7-B8C9-4D5E-2F3A-4B5C6D7E8F9A', 20, '2024-10-10', N'Đã kiểm tra'),
('A1B2C3D4-E5F6-4A5B-8C9D-3E3F3A3B3C3D', 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B', 'C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 20, '2024-10-06', N'Chờ kiểm tra');

-- INSERT dữ liệu cho bảng Evaluate (QC check)
INSERT INTO [TcapsDB].[dbo].[Evaluates] 
(Id, ProductionId, UserId, Status, Note, QuantityError, Image, CreatedAt)
VALUES 
('F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', 'C6D7E8F9-A1B2-4C5D-4E5F-6A7B8C9D1E2F', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', N'Đạt', N'Chất lượng nón tốt, bề mặt nhẵn', 0, N'/images/qc/qc_001.jpg', GETDATE()),
('A1B2C3D4-E5F6-4A5B-8C9D-0E0F0A0B0C0D', 'D7E8F9A1-B2C3-4D5E-5F6A-7B8C9D1E2F3A', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', N'Đạt', N'Nón fullface đạt tiêu chuẩn, khóa cài tốt', 0, N'/images/qc/qc_002.jpg', GETDATE()),
('B2C3D4E5-F6A7-4B5C-9D1E-0F0A0B0C0D0E', 'F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', 'E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B', N'Đạt', N'Sản phẩm đạt yêu cầu, không có lỗi', 0, N'/images/qc/qc_003.jpg', GETDATE());

-- INSERT dữ liệu cho bảng ComponentDefect (QC check)
INSERT INTO [TcapsDB].[dbo].[ComponentDefects] 
(Id, EvaluateId, DefectType, Serverity, Description, Solution, Quantity, CreatedAt, Status)
VALUES 
('B2C3D4E5-F6A7-4B5C-9D1E-0F0A0B0C0D0E', 'F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', N'Vết xước nhỏ', N'Thấp', N'Vết xước nhỏ trên bề mặt vỏ nón', N'Đánh bóng lại bề mặt', 3, GETDATE(), N'Đã xử lý'),
('C3D4E5F6-A7B8-4C5D-1E2F-0A0B0C0D0E0F', 'A1B2C3D4-E5F6-4A5B-8C9D-0E0F0A0B0C0D', N'Khóa cài lỏng', N'Trung bình', N'Khóa cài có độ lỏng nhẹ', N'Điều chỉnh lại khóa cài', 5, GETDATE(), N'Đã xử lý'),
('D4E5F6A7-B8C9-4D5E-2F3A-0B0C0D0E0F0A', 'F9A1B2C3-D4E5-4F5A-7B8C-9D1E2F3A4B5C', N'Màu sơn không đều', N'Thấp', N'Màu sơn có chỗ đậm nhạt không đồng đều', N'Sơn lại lớp hoàn thiện', 2, GETDATE(), N'Đang xử lý'),
('E5F6A7B8-C9D1-4E5F-3A4B-0C0D0E0F0A0B', 'A1B2C3D4-E5F6-4A5B-8C9D-0E0F0A0B0C0D', N'Mút xốp lệch vị trí', N'Cao', N'Mút xốp lót không đúng vị trí thiết kế', N'Tháo ra và lắp lại đúng vị trí', 1, GETDATE(), N'Đã xử lý');

INSERT INTO [TcapsDB].[dbo].[MaterialWorkshops] 
(Id, WorkshopId, QuantitySend, QuantityReceive, Name, Unit, ShipDate, Image, CreatedAt, Status)
VALUES
('A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 1000, 950, N'Vải cotton loại A', N'mét', '2025-11-03', N'https://example.com/image1.jpg', GETDATE(), N'Confirmed'),
('B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 500, 480, N'Sợi nylon 210D', N'cuộn', '2025-11-04', N'https://example.com/image2.jpg', GETDATE(), N'Pending'),
('C3D4E5F6-A7B8-4C5D-1E2F-3A4B5C6D7E8F', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 1200, 1190, N'Dây viền polyester', N'kg', '2025-11-05', N'https://example.com/image3.jpg', GETDATE(), N'Completed');

select * from Evaluates
select * from ComponentDefects
select * from Users
select * from Assignments
select * from Workshop
select * from Productions
select * from Incomes
select * from Notifications
select * from ComponentDefects
select * from MaterialWorkshops
select * from Batches
select * from TaskTransferRequests
select * from MaterialRequests
select * from Materials
update Assignments set EndDate = '2025-11-10'  where Id = 'E2F3A4B5-C6D7-4E5F-9A1B-2C3D4E5F6A7B'