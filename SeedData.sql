-- ============================================
-- SEED DATA với ID liên kết chính xác
-- ============================================

-- Xóa dữ liệu cũ (theo thứ tự phụ thuộc)
DELETE FROM ComponentDefects;
DELETE FROM Evaluates;
DELETE FROM Productions;
DELETE FROM Incomes;
DELETE FROM MaterialUse;
DELETE FROM MaterialRequests;
DELETE FROM Assignments;
DELETE FROM MaterialWorkshops;
DELETE FROM Batches;
DELETE FROM Products;
DELETE FROM Inventories;
DELETE FROM Materials;
DELETE FROM Notifications;
DELETE FROM Users;
DELETE FROM Workshop;

select * from Workshop
INSERT INTO Workshop (Id, Name, Description, StepOrder)
VALUES 
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000001' AS UNIQUEIDENTIFIER), N'Cắt laser (nếu có)', N'Công đoạn Cắt laser (nếu có)', 1),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000002' AS UNIQUEIDENTIFIER), N'Cắt vải ép keo', N'Công đoạn Cắt vải ép keo', 2),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000003' AS UNIQUEIDENTIFIER), N'Dán vải', N'Công đoạn Dán vải', 3),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000004' AS UNIQUEIDENTIFIER), N'Cắt vải ra đủ bộ', N'Công đoạn Cắt vải ra đủ bộ', 4),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000005' AS UNIQUEIDENTIFIER), N'In + thêu', N'Công đoạn In + thêu', 5),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000006' AS UNIQUEIDENTIFIER), N'Làm chỏm', N'Công đoạn Làm chỏm', 6),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000007' AS UNIQUEIDENTIFIER), N'Làm kết', N'Công đoạn Làm kết', 7),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000008' AS UNIQUEIDENTIFIER), N'Đóng nút bấm đuôi (nếu có)', N'Công đoạn Đóng nút bấm đuôi (nếu có)', 8),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000009' AS UNIQUEIDENTIFIER), N'Xỏ cây dựng + vắt sổ', N'Công đoạn Xỏ cây dựng + vắt sổ', 9),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000010' AS UNIQUEIDENTIFIER), N'Vào nón + vào đai', N'Công đoạn Vào nón + vào đai', 10),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000011' AS UNIQUEIDENTIFIER), N'Trần đầu nón', N'Công đoạn Trần đầu nón', 11),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000012' AS UNIQUEIDENTIFIER), N'May đuôi khóa vào nón (nếu có)', N'Công đoạn May đuôi khóa vào nón (nếu có)', 12),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000013' AS UNIQUEIDENTIFIER), N'May tem', N'Công đoạn May tem', 13),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000014' AS UNIQUEIDENTIFIER), N'Cắt chỉ dư + xỏ tem thẻ bài', N'Công đoạn Cắt chỉ dư + xỏ tem thẻ bài', 14),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000015' AS UNIQUEIDENTIFIER), N'Ủi nón', N'Công đoạn Ủi nón', 15),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000016' AS UNIQUEIDENTIFIER), N'Gấp cây + đóng bịch', N'Công đoạn Gấp cây + đóng bịch', 16);

select * from Users
INSERT INTO [Users] (Id, WorkshopId, Role, FullName, Email, PasswordHash, Phone, Status, IsQcTransport, CreatedAt)
VALUES 
-- Admin
('A0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'Admin', N'Nguyễn Văn An', 'admin@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0901234567', 'Active', 0, GETDATE()),
-- Lead
('A0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Lead', N'Trần Thị Bình', 'manager@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0902345678', 'Active', 0, GETDATE()),
-- Staff xưởng 3 (Dán vải)
('A0000000-0000-0000-0000-000000000003', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Lê Văn Cường', 'worker1@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0903456789', 'Active', 0, GETDATE()),
-- Staff xưởng 3 (Dán vải)
('A0000000-0000-0000-0000-000000000004', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Phạm Thị Dung', 'worker2@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0904567890', 'Active', 0, GETDATE()),
-- QC
('A0000000-0000-0000-0000-000000000005', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QC', N'Hoàng Văn Em', 'qc@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0905678901', 'Active', 0, GETDATE()),
-- QC Transport
('A0000000-0000-0000-0000-000000000006', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QCTransport', N'Nguyễn Thị Hạnh', 'qctransport@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0906789012', 'Active', 0, GETDATE()),
-- QC Transport
('A0000000-0000-0000-0000-000000000007', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'QCTransport', N'Nguyễn Văn Bình', 'qctransport2@company.com', '$2a$12$OeyDys0yr8QhSKhfPbzm5u8wQ9cg3lOjS5y2ICxSYXC4PsK4AbjEi', '0901234568', 'Active', 0, GETDATE());

select * from Materials
INSERT INTO [Materials] (Id, Name, Description, Quantity, Price, Unit)
VALUES 
('B0000000-0000-0000-0000-000000000001', N'Vải cotton loại A', N'Vải cotton cao cấp dùng làm nón', 1000, 50000, N'Mét'),
('B0000000-0000-0000-0000-000000000002', N'Sợi nylon 210D', N'Sợi nylon độ bền cao', 500, 80000, N'Kg'),
('B0000000-0000-0000-0000-000000000003', N'Dây viền polyester', N'Dây viền trang trí nón', 300, 30000, N'Kg'),
('B0000000-0000-0000-0000-000000000004', N'Keo dán vải', N'Keo chuyên dụng ép vải', 200, 120000, N'Lít'),
('B0000000-0000-0000-0000-000000000005', N'Mút xốp lót nón', N'Mút xốp êm ái lót bên trong', 800, 25000, N'Miếng'),
('B0000000-0000-0000-0000-000000000006', N'Khóa cài nón', N'Khóa cài nhựa chắc chắn', 1000, 5000, N'Cái');

select * from Products
INSERT INTO [Products] (Id, Code, Name, Image, Description, IsDeleted)
VALUES 
('C0000000-0000-0000-0000-000000000001', 'NON-001', N'Nón bảo hiểm 3/4 đầu', N'/images/products/helmet_34.jpg', N'Nón bảo hiểm 3/4 đầu có kính, chất liệu ABS cao cấp', 0),
('C0000000-0000-0000-0000-000000000002', 'NON-002', N'Nón bảo hiểm fullface', N'/images/products/helmet_fullface.jpg', N'Nón bảo hiểm fullface nguyên đầu, tiêu chuẩn DOT', 0),
('C0000000-0000-0000-0000-000000000003', 'NON-003', N'Nón bảo hiểm nửa đầu', N'/images/products/helmet_half.jpg', N'Nón bảo hiểm nửa đầu thời trang, nhẹ và thoáng khí', 0),
('C0000000-0000-0000-0000-000000000004', 'NON-004', N'Nón bảo hiểm trẻ em', N'/images/products/helmet_kids.jpg', N'Nón bảo hiểm dành cho trẻ em, nhiều màu sắc', 0),
('C0000000-0000-0000-0000-000000000005', 'NON-005', N'Nón bảo hiểm thể thao', N'/images/products/helmet_sport.jpg', N'Nón bảo hiểm thể thao Motocross, chất liệu composite', 0);

select * from Batches
INSERT INTO [Batches] (Id, ProductId, Code, Quantity, StartDate, EndDate, CreatedAt, Status, isDeleted)
VALUES 
-- Lô 1: NON-001, 100 cái, Đang sản xuất
('D0000000-0000-0000-0000-000000000001', 'C0000000-0000-0000-0000-000000000001', 'BATCH-2025-001', 100, '2025-10-01', '2025-11-12', GETDATE(), N'Completed', 0),
-- Lô 2: NON-002, 50 cái, Đang sản xuất
('D0000000-0000-0000-0000-000000000002', 'C0000000-0000-0000-0000-000000000002', 'BATCH-2025-002', 50, '2025-11-05', '2025-11-20', GETDATE(), N'Planned', 0),
-- Lô 3: NON-001, 150 cái, Chưa bắt đầu
('D0000000-0000-0000-0000-000000000003', 'C0000000-0000-0000-0000-000000000001', 'BATCH-2025-003', 150, '2025-10-10', '2025-11-02', GETDATE(), N'Completed', 0),
-- Lô 4: NON-003, 80 cái, Chưa bắt đầu
('D0000000-0000-0000-0000-000000000004', 'C0000000-0000-0000-0000-000000000003', 'BATCH-2025-004', 80, '2025-11-12', '2025-11-30', GETDATE(), N'InProgress', 0),
-- Lô 5: NON-004, 120 cái, Hoàn thành
('D0000000-0000-0000-0000-000000000005', 'C0000000-0000-0000-0000-000000000004', 'BATCH-2025-005', 120, '2025-11-01', '2025-12-28', GETDATE(), N'InProgress', 0);

select * from Assignments
INSERT INTO [Assignments] (Id, BatchId, WorkshopId, StepOrder, Quantity, UnitPrice, StartDate, EndDate, ExpectedDeliveryDate, RequiresMaterialDelivery, Status, CreatedAt)
VALUES 
-- Lô 1 (D001): Công đoạn 1 - Cắt laser
('E0000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 1, 100, 10000, '2025-11-01', '2025-11-03', '2025-11-03', 1, N'InProgress', GETDATE()),
-- Lô 1 (D001): Công đoạn 2 - Cắt vải ép keo
('E0000000-0000-0000-0000-000000000002', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 2, 100, 15000, '2025-11-04', '2025-11-06', '2025-11-06', 1, N'InProgress', GETDATE()),
-- Lô 1 (D001): Công đoạn 3 - Dán vải
('E0000000-0000-0000-0000-000000000003', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 3, 100, 20000, '2025-11-07', '2025-11-09', '2025-11-09', 1, N'InProgress', GETDATE()),
-- Lô 1 (D001): Công đoạn 4 - Cắt vải ra đủ bộ
('E0000000-0000-0000-0000-000000000004', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 4, 100, 12000, '2025-11-10', '2025-11-12', '2025-11-12', 0, N'InProgress', GETDATE()),

-- Lô 2 (D002): Công đoạn 1 - Cắt laser
('E0000000-0000-0000-0000-000000000005', 'D0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 1, 50, 10000, '2025-11-05', '2025-11-07', '2025-11-07', 1, N'InProgress', GETDATE()),
-- Lô 2 (D002): Công đoạn 2 - Cắt vải ép keo
('E0000000-0000-0000-0000-000000000006', 'D0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 2, 50, 15000, '2025-11-08', '2025-11-10', '2025-11-10', 1, N'InProgress', GETDATE()),

-- Lô 5 (D005): 
('E0000000-0000-0000-0000-000000000007', 'D0000000-0000-0000-0000-000000000005', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 16, 120, 8000, '2025-11-06', '2025-11-20', '2025-11-06', 0, N'InProgress', GETDATE());

select * from MaterialRequests
INSERT INTO MaterialRequests (Id, MaterialId, UserId, BatchId, AssignId, QuantityRequest, Status, Note, Date, Type)
VALUES 
-- Staff worker1 yêu cầu vải cotton cho Lô 1, Assignment 1
('F0000000-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000003', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', 200, N'Đã duyệt', N'Cần gấp cho công đoạn cắt laser', '2025-11-01', 'LeadExport'),
-- Staff worker1 yêu cầu keo dán cho Lô 1, Assignment 2
('F0000000-0000-0000-0000-000000000002', 'B0000000-0000-0000-0000-000000000004', 'A0000000-0000-0000-0000-000000000003', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000002', 50, N'Đã duyệt', N'Keo ép vải', '2025-11-04', 'QcAddMaterial'),
-- Staff worker2 yêu cầu sợi nylon cho Lô 2, Assignment 5
('F0000000-0000-0000-0000-000000000003', 'B0000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000004', 'D0000000-0000-0000-0000-000000000002', 'E0000000-0000-0000-0000-000000000005', 100, N'Chờ duyệt', N'Sợi nylon cho fullface', '2025-11-05', 'QcAddMaterial');

select * from Inventories
INSERT INTO [Inventories] (Id, MaterialId, Quantity, Date, Price, ImageURL)
VALUES 
('10000000-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 800, CAST(GETDATE() AS DATE), 50000, 'https://example.com/images/inventory1.jpg'),
('10000000-0000-0000-0000-000000000002', 'B0000000-0000-0000-0000-000000000002', 400, CAST(GETDATE() AS DATE), 80000, 'https://example.com/images/inventory2.jpg'),
('10000000-0000-0000-0000-000000000003', 'B0000000-0000-0000-0000-000000000003', 280, CAST(GETDATE() AS DATE), 30000, 'https://example.com/images/inventory3.jpg'),
('10000000-0000-0000-0000-000000000004', 'B0000000-0000-0000-0000-000000000004', 150, CAST(GETDATE() AS DATE), 120000, 'https://example.com/images/inventory4.jpg'),
('10000000-0000-0000-0000-000000000005', 'B0000000-0000-0000-0000-000000000005', 750, CAST(GETDATE() AS DATE), 25000, 'https://example.com/images/inventory5.jpg'),
('10000000-0000-0000-0000-000000000006', 'B0000000-0000-0000-0000-000000000006', 900, CAST(GETDATE() AS DATE), 5000, 'https://example.com/images/inventory6.jpg');

select * from MaterialUse
INSERT INTO [MaterialUse] (Id, MaterialId, BatchId, AssignId, QuantityDivide, QuantityStaffUse, ReconciledQuantity, QuantityRequest, Date)
VALUES 
-- Lô 1, Assignment 1: Đã dùng vải cotton
('20000000-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', 200, 190, 185, 200, '2025-11-02'),
-- Lô 1, Assignment 2: Đã dùng keo dán
('20000000-0000-0000-0000-000000000002', 'B0000000-0000-0000-0000-000000000004', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000002', 50, 45, 43, 50, '2025-11-05'),
-- Lô 2, Assignment 5: Đã dùng vải cotton
('20000000-0000-0000-0000-000000000003', 'B0000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000002', 'E0000000-0000-0000-0000-000000000005', 100, 95, 92, 100, '2025-11-06'),
-- Lô 5, Assignment 3: 
('20000000-0000-0000-0000-000000000004', 'B0000000-0000-0000-0000-000000000003', 'D0000000-0000-0000-0000-000000000005', 'E0000000-0000-0000-0000-000000000007', 200, 0, 0, 0, '2025-11-02')

select * from MaterialWorkshops
INSERT INTO [MaterialWorkshops] (Id, WorkshopId, AssignId, QuantitySend, QuantityReceive, ShipDate, CreatedAt, Status)
VALUES
-- Giao vải cotton cho xưởng 1
('30000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'E0000000-0000-0000-0000-000000000001', 200, 190, '2025-11-01', GETDATE(), N'Confirmed'),
-- Giao keo dán cho xưởng 2
('30000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'E0000000-0000-0000-0000-000000000002', 50, 45, '2025-11-04', GETDATE(), N'Confirmed'),
-- Giao sợi nylon cho xưởng 1
('30000000-0000-0000-0000-000000000003', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 'E0000000-0000-0000-0000-000000000003', 100, 95, '2025-11-05', GETDATE(), N'Confirmed');

select * from Productions
INSERT INTO [Productions] (Id, AssignId, UserId, Quantity, Date, Time, Status)
VALUES 
-- Worker1 hoàn thành 100 sản phẩm ở Assignment 1 (Lô 1)
('40000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000003', 100, '2025-11-03', '08:30:00', N'Passed'),
-- Worker1 hoàn thành 50 sản phẩm ở Assignment 2 (Lô 1) - chưa kiểm tra
('40000000-0000-0000-0000-000000000002', 'E0000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000003', 50, '2025-11-06', '10:15:00', N'Passed'),
-- Worker2 hoàn thành 50 sản phẩm ở Assignment 5 (Lô 2)
('40000000-0000-0000-0000-000000000003', 'E0000000-0000-0000-0000-000000000005', 'A0000000-0000-0000-0000-000000000004', 50, '2025-11-07', '09:45:00', N'Passed'),
-- Worker2 hoàn thành 120 sản phẩm ở Assignment 7 (Lô 5 - Hoàn thành)
('40000000-0000-0000-0000-000000000004', 'E0000000-0000-0000-0000-000000000005', 'A0000000-0000-0000-0000-000000000004', 120, '2025-11-08', '14:00:00', N'Rework');

select * from Evaluates
INSERT INTO [Evaluates] (Id, ProductionId, UserId, Status, Note, QuantityError, QuantitySuccess, Image, CreatedAt)
VALUES 
-- QC kiểm tra Production 1 - Đạt
('50000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000005', N'Đạt', N'Chất lượng tốt, không có lỗi', 0, 100, N'/images/qc/qc_001.jpg', GETDATE()),
-- QC kiểm tra Production 3 - Đạt
('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003', 'A0000000-0000-0000-0000-000000000005', N'Đạt', N'Fullface đạt tiêu chuẩn', 0, 50, N'/images/qc/qc_002.jpg', GETDATE()),
-- QC kiểm tra Production 4 - Có lỗi nhỏ
('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004', 'A0000000-0000-0000-0000-000000000005', N'Có lỗi', N'Phát hiện một số lỗi nhỏ', 5, 115, N'/images/qc/qc_003.jpg', GETDATE());

INSERT INTO [ComponentDefects] (Id, EvaluateId, DefectType, Serverity, Description, Solution, Quantity, CreatedAt, Status)
VALUES 
-- Lỗi ở Evaluate 3 (Production 4)
('60000000-0000-0000-0000-000000000001', '50000000-0000-0000-0000-000000000003', N'Vết xước nhỏ', N'Thấp', N'Vết xước nhỏ trên bề mặt nón', N'Đánh bóng lại', 3, GETDATE(), N'Đã xử lý'),
('60000000-0000-0000-0000-000000000002', '50000000-0000-0000-0000-000000000003', N'Khóa cài lỏng', N'Trung bình', N'Khóa cài có độ lỏng nhẹ', N'Điều chỉnh lại', 2, GETDATE(), N'Đang xử lý');

select * from Incomes
INSERT INTO [Incomes] (Id, BatchId, ProductionId, UserId, Quantity, TotalPrice, CreatedAt)
VALUES 
-- Thu nhập của Worker1 từ Production 1
('70000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000003', 100, 1000000, GETDATE()),
-- Thu nhập của Worker1 từ Production 2
('70000000-0000-0000-0000-000000000002', 'D0000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000003', 50, 750000, GETDATE()),
-- Thu nhập của Worker2 từ Production 3
('70000000-0000-0000-0000-000000000003', 'D0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003', 'A0000000-0000-0000-0000-000000000004', 50, 500000, GETDATE()),
-- Thu nhập của Worker2 từ Production 4
('70000000-0000-0000-0000-000000000004', 'D0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004', 'A0000000-0000-0000-0000-000000000004', 120, 960000, GETDATE());

select * from Notifications
INSERT INTO [Notifications] (Id, UserId, Title, Message, Type, IsRead, CreatedAt)
VALUES 
('80000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000003', N'Chào mừng', N'Chào mừng bạn đến với hệ thống sản xuất', N'System', 0, GETDATE()),
('80000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000003', N'Nhiệm vụ mới', N'Bạn có nhiệm vụ sản xuất lô BATCH-2025-001', N'Task', 0, GETDATE()),
('80000000-0000-0000-0000-000000000003', 'A0000000-0000-0000-0000-000000000004', N'Nhiệm vụ mới', N'Bạn có nhiệm vụ sản xuất lô BATCH-2025-002', N'Task', 0, GETDATE()),
('80000000-0000-0000-0000-000000000004', 'A0000000-0000-0000-0000-000000000005', N'Yêu cầu kiểm tra', N'Có sản phẩm mới cần kiểm tra chất lượng', N'QC', 1, GETDATE()),
('80000000-0000-0000-0000-000000000005', 'A0000000-0000-0000-0000-000000000002', N'Báo cáo tuần', N'Vui lòng nộp báo cáo sản xuất tuần này', N'Report', 1, GETDATE());

select * from AssignmentTransferRequests
INSERT INTO [TcapsDB].[dbo].[AssignmentTransferRequests]
    ([Id], [AssignmentId], [ReworkRequestId], [UserId], [CompletedQuantity], [Status], [Note], [CreatedAt])
VALUES
    ('a1b2c3d4-e5f6-7890-1234-56789abcdef0', 'E0000000-0000-0000-0000-000000000001', NULL, 'A0000000-0000-0000-0000-000000000005', 50, 'PendingApproval', N'Yêu cầu chuyển giao lần 1', GETDATE()),
    ('b2c3d4e5-f6a1-8901-2345-6789abcdef01', 'E0000000-0000-0000-0000-000000000002', null, 'A0000000-0000-0000-0000-000000000005', 75, 'PendingApproval', N'Hoàn tất chuyển giao', GETDATE());

select * from WorkshopInventory
INSERT INTO [TcapsDB].[dbo].[WorkshopInventory]
    ([Id], [WorkshopId], [MaterialId], [Quantity])
VALUES
    ('01B2C3D4-E5F6-7890-ABCD-EF1234567890', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'B0000000-0000-0000-0000-000000000001', 100),
    ('02C3D4E5-F6A7-8901-BCDE-F12345678901', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'B0000000-0000-0000-0000-000000000003', 50),
    ('03D4E5F6-A7B8-9012-CDEF-123456789012', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'B0000000-0000-0000-0000-000000000001', 75);

select * from TaskTransferRequests
INSERT INTO [TcapsDB].[dbo].[TaskTransferRequests]
    (Id, BatchId, WorkshopId, QcTransportId, Status, Note, CreatedAt, ApprovedAt)
VALUES
    ('A3F2504E-4F89-11D3-9A0C-0305E82C3301', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'A0000000-0000-0000-0000-000000000006', 'Pending', 'Test insert 1', GETDATE(), NULL),
    ('B3F2504E-4F89-11D3-9A0C-0305E82C3302', 'D0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'A0000000-0000-0000-0000-000000000006', 'Approved', 'Test insert 2', GETDATE(), GETDATE()),
    ('C3F2504E-4F89-11D3-9A0C-0305E82C3303', 'D0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'A0000000-0000-0000-0000-000000000007', 'Approved', 'Test insert 3', GETDATE(), NULL);

select * from Users
select * from Evaluates
select * from Assignments
select * from Productions
select * from Products
select * from Batches
select * from MaterialUse
select * from Materials
select * from Incomes
select * from Workshop
select * from Notifications where UserId = 'A0000000-0000-0000-0000-000000000005'
select * from TaskTransferRequests
select * from MaterialSupplies
select * from MaterialWorkshops
select * from MaterialRequests
select * from TaskTransferRequests
select * from ReworkRequests
select * from WorkshopInventory

update MaterialSupplies set WorkshopId = 'A1C9B3A0-4F12-4E81-B17B-000000000003' where Id = '5C7F8137-AF53-4FD3-B9F6-5EE5DEA4A6BE'
update MaterialSupplies set SupplierId = 'A0000000-0000-0000-0000-000000000006' where Id = '5C7F8137-AF53-4FD3-B9F6-5EE5DEA4A6BE'