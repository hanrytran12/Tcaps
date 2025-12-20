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
DELETE FROM TaskTransferRequests
DELETE FROM AssignmentTransferRequests
DELETE FROM Materials
DELETE FROM MaterialSupplies
DELETE FROM ReworkRequests
DELETE FROM WorkshopInventory

select * from Workshop
INSERT INTO Workshop (Id, Name, Description, StepOrder, WorkshopType, Status, CreatedAt)
VALUES 
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000001' AS UNIQUEIDENTIFIER), N'Cắt laser (nếu có)', N'Công đoạn Cắt laser (nếu có)', 1, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000002' AS UNIQUEIDENTIFIER), N'Cắt vải ép keo', N'Công đoạn Cắt vải ép keo', 2, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000003' AS UNIQUEIDENTIFIER), N'Dán vải', N'Công đoạn Dán vải', 3, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000004' AS UNIQUEIDENTIFIER), N'Cắt vải ra đủ bộ', N'Công đoạn Cắt vải ra đủ bộ', 4, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000005' AS UNIQUEIDENTIFIER), N'In + thêu', N'Công đoạn In + thêu', 5, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000006' AS UNIQUEIDENTIFIER), N'Làm chỏm', N'Công đoạn Làm chỏm', 6, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000007' AS UNIQUEIDENTIFIER), N'Làm kết', N'Công đoạn Làm kết', 7, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000008' AS UNIQUEIDENTIFIER), N'Đóng nút bấm đuôi (nếu có)', N'Công đoạn Đóng nút bấm đuôi (nếu có)', 8, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000009' AS UNIQUEIDENTIFIER), N'Xỏ cây dựng + vắt sổ', N'Công đoạn Xỏ cây dựng + vắt sổ', 9, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000010' AS UNIQUEIDENTIFIER), N'Vào nón + vào đai', N'Công đoạn Vào nón + vào đai', 10, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000011' AS UNIQUEIDENTIFIER), N'Trần đầu nón', N'Công đoạn Trần đầu nón', 11, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000012' AS UNIQUEIDENTIFIER), N'May đuôi khóa vào nón (nếu có)', N'Công đoạn May đuôi khóa vào nón (nếu có)', 12, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000013' AS UNIQUEIDENTIFIER), N'May tem', N'Công đoạn May tem', 13, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000014' AS UNIQUEIDENTIFIER), N'Cắt chỉ dư + xỏ tem thẻ bài', N'Công đoạn Cắt chỉ dư + xỏ tem thẻ bài', 14, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000015' AS UNIQUEIDENTIFIER), N'Ủi nón', N'Công đoạn Ủi nón', 15, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000016' AS UNIQUEIDENTIFIER), N'Gấp cây + đóng bịch', N'Công đoạn Gấp cây + đóng bịch', 16, 1, 'Assigned', GETDATE()),
(CAST('A1C9B3A0-4F12-4E81-B17B-000000000017' AS UNIQUEIDENTIFIER), N'Xưởng khoán', N'Xưởng làm tất cả', null, 2, 'Assigned', GETDATE());

select * from Users
INSERT INTO [Users] (Id, WorkshopId, Role, FullName, Email, PasswordHash, Phone, Status, IsQcTransport, CreatedAt)
VALUES 
-- Admin
('A0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'Admin', N'Nguyễn Văn An', 'admin@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901234567', 'Active', 0, GETDATE()),
-- Lead
('A0000000-0000-0000-0000-000000000002', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Lead', N'Trần Thị Bình', 'lead@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902345678', 'Active', 0, GETDATE()),
-- Staff xưởng 3 (Dán vải)
('A0000000-0000-0000-0000-000000000003', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Staff', N'Lê Văn Cường', 'staff.cuong@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0903456789', 'Active', 0, GETDATE()),
-- Staff xưởng 3 (Dán vải)
('A0000000-0000-0000-0000-000000000004', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Staff', N'Phạm Thị Dung', 'staff.dung@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0904567890', 'Active', 0, GETDATE()),
-- QC Transport
('A0000000-0000-0000-0000-000000000005', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QCTransport', N'Nguyễn Thị Hạnh', 'qctransport@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0906789012', 'Active', 0, GETDATE()),

-- QC cho 16 xưởng
-- Step 1: Cắt laser
('A0000000-0000-0000-0000-000000000006', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'QC', N'QC Xưởng Cắt Laser', 'qc.laser@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000101', 'Active', 0, GETDATE()),
-- Step 2: Cắt vải ép keo
('A0000000-0000-0000-0000-000000000007', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'QC', N'Hoàng Văn Em', 'qc.catkeo@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000102', 'Active', 0, GETDATE()),
-- Step 3: Dán vải
('A0000000-0000-0000-0000-000000000008', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'QC', N'Khánh Nguyệt', 'qc.danvai@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000103', 'Active', 0, GETDATE()),
-- Step 4: Cắt vải ra đủ bộ
('A0000000-0000-0000-0000-000000000009', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 'QC', N'QC Xưởng Cắt Đủ Bộ', 'qc.catdubo@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000104', 'Active', 0, GETDATE()),
-- Step 5: In + thêu
('A0000000-0000-0000-0000-000000000010', 'A1C9B3A0-4F12-4E81-B17B-000000000005', 'QC', N'Anh Dũng', 'qc.intheu@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000105', 'Active', 0, GETDATE()),
-- Step 6: Làm chỏm
('A0000000-0000-0000-0000-000000000011', 'A1C9B3A0-4F12-4E81-B17B-000000000006', 'QC', N'QC Xưởng Làm Chỏm', 'qc.lamchom@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000106', 'Active', 0, GETDATE()),
-- Step 7: Làm kết
('A0000000-0000-0000-0000-000000000012', 'A1C9B3A0-4F12-4E81-B17B-000000000007', 'QC', N'QC Xưởng Làm Kết', 'qc.lamket@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000107', 'Active', 0, GETDATE()),
-- Step 8: Đóng nút bấm đuôi
('A0000000-0000-0000-0000-000000000013', 'A1C9B3A0-4F12-4E81-B17B-000000000008', 'QC', N'QC Xưởng Đóng Nút', 'qc.dongnut@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000108', 'Active', 0, GETDATE()),
-- Step 9: Xỏ cây dựng + vắt sổ
('A0000000-0000-0000-0000-000000000014', 'A1C9B3A0-4F12-4E81-B17B-000000000009', 'QC', N'QC Xưởng Vắt Sổ', 'qc.vatso@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000109', 'Active', 0, GETDATE()),
-- Step 10: Vào nón + vào đai
('A0000000-0000-0000-0000-000000000015', 'A1C9B3A0-4F12-4E81-B17B-000000000010', 'QC', N'QC Xưởng Vào Đai', 'qc.vaodai@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000110', 'Active', 0, GETDATE()),
-- Step 11: Trần đầu nón
('A0000000-0000-0000-0000-000000000016', 'A1C9B3A0-4F12-4E81-B17B-000000000011', 'QC', N'QC Xưởng Trần Đầu', 'qc.trandau@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000111', 'Active', 0, GETDATE()),
-- Step 12: May đuôi khóa vào nón
('A0000000-0000-0000-0000-000000000017', 'A1C9B3A0-4F12-4E81-B17B-000000000012', 'QC', N'QC Xưởng May Khóa', 'qc.maykhoa@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000112', 'Active', 0, GETDATE()),
-- Step 13: May tem
('A0000000-0000-0000-0000-000000000018', 'A1C9B3A0-4F12-4E81-B17B-000000000013', 'QC', N'QC Xưởng May Tem', 'qc.maytem@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000113', 'Active', 0, GETDATE()),
-- Step 14: Cắt chỉ dư + xỏ tem thẻ bài
('A0000000-0000-0000-0000-000000000019', 'A1C9B3A0-4F12-4E81-B17B-000000000014', 'QC', N'QC Xưởng Cắt Chỉ', 'qc.catchi@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000114', 'Active', 0, GETDATE()),
-- Step 15: Ủi nón
('A0000000-0000-0000-0000-000000000020', 'A1C9B3A0-4F12-4E81-B17B-000000000015', 'QC', N'QC Xưởng Ủi Nón', 'qc.uinon@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000115', 'Active', 0, GETDATE()),
-- Step 16: Gấp cây + đóng bịch
('A0000000-0000-0000-0000-000000000021', 'A1C9B3A0-4F12-4E81-B17B-000000000016', 'QC', N'QC Xưởng Đóng Bịch', 'qc.dongbich@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000116', 'Active', 0, GETDATE()),
-- Xưởng khoán
('A0000000-0000-0000-0000-000000000056', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'QC', N'QC Xưởng Khoán', 'qc.khoan@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0901000116', 'Active', 0, GETDATE());

INSERT INTO [Users] (Id, WorkshopId, Role, FullName, Email, PasswordHash, Phone, Status, IsQcTransport, CreatedAt)
VALUES
-- Step 1: Cắt laser (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000001)
('A0000000-0000-0000-0000-000000000022', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'Staff', N'Nguyễn Văn A - Laser', 'staff.laser1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902010101', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000023', 'A1C9B3A0-4F12-4E81-B17B-000000000001', 'Staff', N'Trần Thị B - Laser', 'staff.laser2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902010102', 'Active', 0, GETDATE()),

-- Step 2: Cắt vải ép keo (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000002)
('A0000000-0000-0000-0000-000000000024', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Staff', N'Lê Văn C - Cắt Keo', 'staff.catkeo1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902020201', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000025', 'A1C9B3A0-4F12-4E81-B17B-000000000002', 'Staff', N'Phạm Thị D - Cắt Keo', 'staff.catkeo2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902020202', 'Active', 0, GETDATE()),

-- Step 3: Dán vải (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000003)
('A0000000-0000-0000-0000-000000000026', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Hoàng Văn E - Dán Vải', 'staff.danvai1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902030301', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000027', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'Staff', N'Nguyễn Thị F - Dán Vải', 'staff.danvai2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902030302', 'Active', 0, GETDATE()),

-- Step 4: Cắt vải ra đủ bộ (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000004)
('A0000000-0000-0000-0000-000000000028', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 'Staff', N'Phan Văn G - Cắt Đủ Bộ', 'staff.catdubo1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902040401', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000029', 'A1C9B3A0-4F12-4E81-B17B-000000000004', 'Staff', N'Vũ Thị H - Cắt Đủ Bộ', 'staff.catdubo2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902040402', 'Active', 0, GETDATE()),

-- Step 5: In + thêu (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000005)
('A0000000-0000-0000-0000-000000000030', 'A1C9B3A0-4F12-4E81-B17B-000000000005', 'Staff', N'Đỗ Văn I - In Thêu', 'staff.intheu1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902050501', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000031', 'A1C9B3A0-4F12-4E81-B17B-000000000005', 'Staff', N'Trương Thị K - In Thêu', 'staff.intheu2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902050502', 'Active', 0, GETDATE()),

-- Step 6: Làm chỏm (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000006)
('A0000000-0000-0000-0000-000000000032', 'A1C9B3A0-4F12-4E81-B17B-000000000006', 'Staff', N'Nguyễn Văn L - Làm Chỏm', 'staff.lamchom1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902060601', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000033', 'A1C9B3A0-4F12-4E81-B17B-000000000006', 'Staff', N'Trần Thị M - Làm Chỏm', 'staff.lamchom2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902060602', 'Active', 0, GETDATE()),

-- Step 7: Làm kết (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000007)
('A0000000-0000-0000-0000-000000000034', 'A1C9B3A0-4F12-4E81-B17B-000000000007', 'Staff', N'Lê Văn N - Làm Kết', 'staff.lamket1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902070701', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000035', 'A1C9B3A0-4F12-4E81-B17B-000000000007', 'Staff', N'Phạm Thị O - Làm Kết', 'staff.lamket1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902070702', 'Active', 0, GETDATE()),

-- Step 8: Đóng nút bấm đuôi (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000008)
('A0000000-0000-0000-0000-000000000036', 'A1C9B3A0-4F12-4E81-B17B-000000000008', 'Staff', N'Hoàng Văn P - Đóng Nút', 'staff.dongnut1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902080801', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000037', 'A1C9B3A0-4F12-4E81-B17B-000000000008', 'Staff', N'Nguyễn Thị Q - Đóng Nút', 'staff.dongnut2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902080802', 'Active', 0, GETDATE()),

-- Step 9: Xỏ cây dựng + vắt sổ (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000009)
('A0000000-0000-0000-0000-000000000038', 'A1C9B3A0-4F12-4E81-B17B-000000000009', 'Staff', N'Phan Văn R - Vắt Sổ', 'staff.vatso1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902090901', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000039', 'A1C9B3A0-4F12-4E81-B17B-000000000009', 'Staff', N'Vũ Thị S - Vắt Sổ', 'staff.vatso2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902090902', 'Active', 0, GETDATE()),

-- Step 10: Vào nón + vào đai (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000010)
('A0000000-0000-0000-0000-000000000040', 'A1C9B3A0-4F12-4E81-B17B-000000000010', 'Staff', N'Đỗ Văn T - Vào Đai', 'staff.vaodai1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902101001', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000041', 'A1C9B3A0-4F12-4E81-B17B-000000000010', 'Staff', N'Trương Thị U - Vào Đai', 'staff.vaodai2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902101002', 'Active', 0, GETDATE()),

-- Step 11: Trần đầu nón (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000011)
('A0000000-0000-0000-0000-000000000042', 'A1C9B3A0-4F12-4E81-B17B-000000000011', 'Staff', N'Nguyễn Văn V - Trần Đầu', 'staff.trandau1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902111101', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000043', 'A1C9B3A0-4F12-4E81-B17B-000000000011', 'Staff', N'Trần Thị X - Trần Đầu', 'staff.trandau2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902111102', 'Active', 0, GETDATE()),

-- Step 12: May đuôi khóa vào nón (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000012)
('A0000000-0000-0000-0000-000000000044', 'A1C9B3A0-4F12-4E81-B17B-000000000012', 'Staff', N'Lê Văn Y - May Khóa', 'staff.maykhoa1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902121201', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000045', 'A1C9B3A0-4F12-4E81-B17B-000000000012', 'Staff', N'Phạm Thị Z - May Khóa', 'staff.maykhoa2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902121202', 'Active', 0, GETDATE()),

-- Step 13: May tem (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000013)
('A0000000-0000-0000-0000-000000000046', 'A1C9B3A0-4F12-4E81-B17B-000000000013', 'Staff', N'Hoàng Văn AA - May Tem', 'staff.maytem1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902131301', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000047', 'A1C9B3A0-4F12-4E81-B17B-000000000013', 'Staff', N'Nguyễn Thị BB - May Tem', 'staff.maytem2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902131302', 'Active', 0, GETDATE()),

-- Step 14: Cắt chỉ dư + xỏ tem thẻ bài (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000014)
('A0000000-0000-0000-0000-000000000048', 'A1C9B3A0-4F12-4E81-B17B-000000000014', 'Staff', N'Phan Văn CC - Cắt Chỉ', 'staff.catchi1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902141401', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000049', 'A1C9B3A0-4F12-4E81-B17B-000000000014', 'Staff', N'Vũ Thị DD - Cắt Chỉ', 'staff.catchi2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902141402', 'Active', 0, GETDATE()),

-- Step 15: Ủi nón (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000015)
('A0000000-0000-0000-0000-000000000050', 'A1C9B3A0-4F12-4E81-B17B-000000000015', 'Staff', N'Đỗ Văn EE - Ủi Nón', 'staff.uinon1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902151501', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000051', 'A1C9B3A0-4F12-4E81-B17B-000000000015', 'Staff', N'Trương Thị FF - Ủi Nón', 'staff.uinon2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902151502', 'Active', 0, GETDATE()),

-- Step 16: Gấp cây + đóng bịch (WorkshopId: A1C9B3A0-4F12-4E81-B17B-000000000016)
('A0000000-0000-0000-0000-000000000052', 'A1C9B3A0-4F12-4E81-B17B-000000000016', 'Staff', N'Nguyễn Văn GG - Đóng Bịch', 'staff.dongbich1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161601', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000053', 'A1C9B3A0-4F12-4E81-B17B-000000000016', 'Staff', N'Trần Thị HH - Đóng Bịch', 'staff.dongbich2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE()),

-- Nhân viên xưởng khoán
('A0000000-0000-0000-0000-000000000054', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Staff', N'Nguyễn Văn GG - Khoán', 'staff.khoan1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161601', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000055', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Staff', N'Trần Thị HH - Khoán', 'staff.khoan2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE()),

-- QC gác cổng
('A0000000-0000-0000-0000-000000000057', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'GuardQC', N'QC Gác Cổng', 'qc.gaccong@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE());


-- Nhân viên xưởng khoán
('A0000000-0000-0000-0000-000000000054', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Staff', N'Nguyễn Văn GG - Khoán', 'staff.khoan1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161601', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000055', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Staff', N'Trần Thị HH - Khoán', 'staff.khoan2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE()),

-- QC gác cổng
('A0000000-0000-0000-0000-000000000057', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'GuardQC', N'QC Gác Cổng', 'qc.gaccong@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE()),
-- Lead
('A0000000-0000-0000-0000-000000000058', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Lead', N'Lead1', 'lead1@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE()),
('A0000000-0000-0000-0000-000000000059', 'A1C9B3A0-4F12-4E81-B17B-000000000017', 'Lead', N'Lead2', 'lead2@tcaps.com', '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC', '0902161602', 'Active', 0, GETDATE());
-- Đổi tất cả mật khẩu thành "123"
--UPDATE [Users] 
--SET PasswordHash = '$2a$11$tVSQZ.QyXTekMK9jnqwhWuM69Hnwiubpy1whI.uLRR4.HYRaJPwwC'
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
INSERT INTO [Products] (Id, Code, Name, Image, Description, IsDeleted, CreatedAt)
VALUES 
('C0000000-0000-0000-0000-000000000001', 'NON-001', N'Nón bảo hiểm 3/4 đầu', N'/images/products/helmet_34.jpg', N'Nón bảo hiểm 3/4 đầu có kính, chất liệu ABS cao cấp', 0, ''),
('C0000000-0000-0000-0000-000000000002', 'NON-002', N'Nón bảo hiểm fullface', N'/images/products/helmet_fullface.jpg', N'Nón bảo hiểm fullface nguyên đầu, tiêu chuẩn DOT', 0, ''),
('C0000000-0000-0000-0000-000000000003', 'NON-003', N'Nón bảo hiểm nửa đầu', N'/images/products/helmet_half.jpg', N'Nón bảo hiểm nửa đầu thời trang, nhẹ và thoáng khí', 0, ''),
('C0000000-0000-0000-0000-000000000004', 'NON-004', N'Nón bảo hiểm trẻ em', N'/images/products/helmet_kids.jpg', N'Nón bảo hiểm dành cho trẻ em, nhiều màu sắc', 0, ''),
('C0000000-0000-0000-0000-000000000005', 'NON-005', N'Nón bảo hiểm thể thao', N'/images/products/helmet_sport.jpg', N'Nón bảo hiểm thể thao Motocross, chất liệu composite', 0, '');

select * from Batches
INSERT INTO [Batches] (Id, ProductId, UserId, Code, Quantity, ActualQuantity, LostQuantity, StartDate, EndDate, CreatedAt, Status, isDeleted)
VALUES 
-- Lô 1: NON-001, 100 cái, Đang sản xuất
('D0000000-0000-0000-0000-000000000001', 'C0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000002', 'BATCH-2025-001', 100, 100, 0, '2025-10-01', '2025-11-12', GETDATE(), N'Completed', 0);

select * from Assignments
INSERT INTO [Assignments] (Id, BatchId, WorkshopId, StepOrder, Quantity, UnitPrice, StartDate, EndDate, ExpectedDeliveryDate, DateCompleted, RequiresMaterialDelivery, Status, CreatedAt)
VALUES 
-- Lô 1 (D001): Công đoạn 1 - Cắt laser
('E0000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 1, 100, 10000, '2025-11-01', '2025-11-03', '2025-11-03', null, 1, N'Completed', GETDATE());

select * from MaterialRequests
INSERT INTO MaterialRequests (Id, MaterialId, UserId, BatchId, AssignId, QuantityRequest, Status, Note, Date, Type, NoteFromQC, ActualReceivedQuantity, QuantityFromStock)
VALUES 
-- Staff worker1 yêu cầu vải cotton cho Lô 1, Assignment 1
('F0000000-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000008', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', 200, N'Confirmed', N'Cần gấp cho công đoạn cắt laser', '2025-11-01', 'LeadExport', null, 0, 0);

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
INSERT INTO [MaterialUse] (Id, MaterialId, BatchId, AssignId, ReworkRequestId, QuantityDivide, QuantityStaffUse, ReconciledQuantity, QuantityRequest, Date)
VALUES 
-- Lô 1, Assignment 1: Đã dùng vải cotton
('20000000-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', null, 200, 0, 0, 0, '2025-11-02');

select * from MaterialWorkshops
INSERT INTO [MaterialWorkshops] (Id, WorkshopId, AssignId, AssignmentTransferRequestId, SupplierId, QuantitySend, QuantityReceive, ShipDate, CreatedAt, Status)
VALUES
-- Giao vải cotton cho xưởng 1
('30000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'E0000000-0000-0000-0000-000000000001', 'a1b2c3d4-e5f6-7890-1234-56789abcdef0', 'A0000000-0000-0000-0000-000000000002', 100, 0, '2025-11-01', GETDATE(), N'Confirmed');

select * from Productions
INSERT INTO [Productions] (Id, AssignId, UserId, Quantity, Date, Time, Status, ReworkRequestId)
VALUES 
-- Worker1 hoàn thành 100 sản phẩm ở Assignment 1 (Lô 1)
('40000000-0000-0000-0000-000000000001', 'E0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000026', 50, '2025-11-03', '08:30:00', N'Passed', null),
('40000000-0000-0000-0000-000000000002', 'E0000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000027', 50, '2025-11-04', '08:30:00', N'Passed', null);

select * from Evaluates
INSERT INTO [Evaluates] (Id, ProductionId, UserId, Status, Note, QuantityError, QuantitySuccess, Image, CreatedAt)
VALUES 
-- QC kiểm tra Production 1 - Đạt
('50000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000008', N'Passed', N'Chất lượng tốt, không có lỗi', 0, 100, N'/images/qc/qc_001.jpg', GETDATE()),
-- QC kiểm tra Production 3 - Đạt
('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000008', N'CompleteWithLoss', N'Fullface đạt tiêu chuẩn', 5, 50, N'/images/qc/qc_002.jpg', GETDATE());

select * from ComponentDefects
INSERT INTO [ComponentDefects] (Id, EvaluateId, Description, Quantity, CreatedAt, Status)
VALUES 
-- Lỗi ở Evaluate 3 (Production 4)
('60000000-0000-0000-0000-000000000001', '50000000-0000-0000-0000-000000000002', N'Vết xước nhỏ trên bề mặt nón', 3, GETDATE(), N'Unfixabled'),
('60000000-0000-0000-0000-000000000002', '50000000-0000-0000-0000-000000000002', N'Khóa cài có độ lỏng nhẹ', 2, GETDATE(), N'Confirmed');

select * from Incomes
INSERT INTO [Incomes] (Id, BatchId, ProductionId, UserId, Quantity, TotalPrice, CreatedAt)
VALUES 
-- Thu nhập của Worker1 từ Production 1
('70000000-0000-0000-0000-000000000001', 'D0000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'A0000000-0000-0000-0000-000000000026', 50, 500000, GETDATE()),
-- Thu nhập của Worker1 từ Production 2
('70000000-0000-0000-0000-000000000002', 'D0000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000002', 'A0000000-0000-0000-0000-000000000027', 48, 480000, GETDATE());

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
    ([Id], [AssignmentId], [ReworkRequestId], [UserId], [CompletedQuantitySend], [CompletedQuantityReceive], [Status], [Note], [NoteLead], [CreatedAt])
VALUES
    ('a1b2c3d4-e5f6-7890-1234-56789abcdef0', 'E0000000-0000-0000-0000-000000000001', NULL, 'A0000000-0000-0000-0000-000000000005', 98, 0, 'PendingApproval', N'Yêu cầu chuyển giao lần 1', NULL, GETDATE());

select * from WorkshopInventory
INSERT INTO [TcapsDB].[dbo].[WorkshopInventory]
    ([Id], [WorkshopId], [MaterialId], [Quantity], [HoldingQuantity])
VALUES
    ('01B2C3D4-E5F6-7890-ABCD-EF1234567890', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'B0000000-0000-0000-0000-000000000001', 100, 0);

select * from TaskTransferRequests
INSERT INTO [TcapsDB].[dbo].[TaskTransferRequests]
    (Id, BatchId, WorkshopId, QcTransportId, MaterialRequestId, AssignmentTransferId, Status, Note, CreatedAt, ApprovedAt)
VALUES
    ('A3F2504E-4F89-11D3-9A0C-0305E82C3301', 'D0000000-0000-0000-0000-000000000001', 'A1C9B3A0-4F12-4E81-B17B-000000000003', 'A0000000-0000-0000-0000-000000000005', 'F0000000-0000-0000-0000-000000000001', null, 'Pending', 'Test insert 1', GETDATE(), NULL);

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
select * from AssignmentTransferRequests
select * from ReworkRequests
select * from MaterialUse
select * from FinalTransferRequests

insert into MaterialWorkshops values 
('A0000000-0000-0000-0000-000000000015', 
'A1C9B3A0-4F12-4E81-B17B-000000000003',
'F91BF72A-77D2-4CD5-9DC1-D1E965CDEADC',
100, 95, '2025-11-22', '2025-11-20', 'Approved');

update MaterialSupplies set WorkshopId = 'A1C9B3A0-4F12-4E81-B17B-000000000003' where Id = '5C7F8137-AF53-4FD3-B9F6-5EE5DEA4A6BE'
update Users set WorkshopId = '4B8DE283-FE42-4FDF-99B3-22FDE7B99C27' where Id = 'A0000000-0000-0000-0000-000000000056'