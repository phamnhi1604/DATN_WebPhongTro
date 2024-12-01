use master
go
drop database DB_PhongTro
go
Create database DB_PhongTro
go
use DB_PhongTro

GO
--------------------------------------
CREATE TABLE VaiTro (
    IdVaiTro INT IDENTITY(1,1),
    TenVaiTro NVARCHAR(255) NOT NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (IdVaiTro),
    CONSTRAINT UNI_TenVaiTro UNIQUE(TenVaiTro)
);
GO
INSERT INTO VaiTro VALUES (N'Admin');
INSERT INTO VaiTro VALUES (N'Kiểm duyệt viên');
INSERT INTO VaiTro VALUES (N'Người cho thuê');
INSERT INTO VaiTro VALUES (N'Khách thuê');

CREATE TABLE NguoiDung 
(
    IdNguoiDung BIGINT IDENTITY(1,1),
    TenTaiKhoan VARCHAR(255) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    IdVaiTro INT NOT NULL,
	TonTai BIT NOT NULL,
    CONSTRAINT UNI_TenTaiKhoan UNIQUE(TenTaiKhoan),
    CONSTRAINT PK_NguoiDung PRIMARY KEY (IdNguoiDung),
	CONSTRAINT FK_NguoiDung_VaiTro FOREIGN KEY(IdVaiTro) REFERENCES VaiTro(IdVaiTro)
);

INSERT INTO NguoiDung VALUES ('admin','admin@123',1, 1)
INSERT INTO NguoiDung VALUES ('Kiemduyetvien1','123123123',2, 1)
INSERT INTO NguoiDung VALUES ('Kiemduyetvien2','123123123',2, 1)
INSERT INTO NguoiDung VALUES ('Nguoichothue1','123123123',3, 1)
INSERT INTO NguoiDung VALUES ('Nguoichothue2','123123123',3, 1)
INSERT INTO NguoiDung VALUES ('customer1','123123123',4, 1)
INSERT INTO NguoiDung VALUES ('phamyennhi','qqqqqqqq',4, 1)
 select * from NguoiDung

create table KiemDuyetVien
(
	 IdKDV BIGINT IDENTITY(1,1),
	 TenKDV NVarchar(255),
	 DiaChiKDV NVarchar(255),
	 IdNguoiDung BIGINT not null ,
     CONSTRAINT IdKDV PRIMARY KEY (IdKDV),
	 CONSTRAINT FK_KDV_NguoiDung FOREIGN KEY(IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
)
--thêm thông tin đầy đủ và chạy lại db
INSERT INTO KiemDuyetVien  VALUES 
    (N'Phạm Yến Nhi', N'Bạc Liêu', 2),
    (N'Lê Thị B', N'140 Lê Trọng Tấn, Tân Phú', 3);
select * from KiemDuyetVien

CREATE TABLE NguoiThue
(
	IdNguoiThue  BIGINT IDENTITY(1,1),
	IdNguoiDung BIGINT NOT NULL,
	TenKhachHang NVARCHAR(255) NOT NULL,
	NgaySinh DATE,
	GioiTinh NCHAR(5),
	DiaChi NVARCHAR(255),
	SoDienThoai CHAR(10),
	Email VARCHAR(100),
	CONSTRAINT PK_KhachHang PRIMARY KEY(IdNguoiThue),
	CONSTRAINT UNI_IdNguoiDung_NguoiThue UNIQUE(IdNguoiDung),
	CONSTRAINT UNI_SoDienThoai_NguoiThue UNIQUE(SoDienThoai),
	CONSTRAINT UNI_Email_NguoiThue UNIQUE(Email),
	CONSTRAINT FK_NguoiThue_NguoiDung FOREIGN KEY(IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung),
	
);
SET DATEFORMAT DMY
ALTER TABLE NguoiTHue
ADD CONSTRAINT DF_DiaChi_NguoiThue DEFAULT N'Không xác định' FOR DiaChi;
INSERT INTO NguoiThue (IdNguoiDung, TenKhachHang, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email) VALUES 
    (6, N'Trần Văn C', '15/03/1995', N'Nam', N'789 Đường GHI, Đà Nẵng', '0901123456', 'customer1@gmail.com');
select * from NguoiThue
SET DATEFORMAT DMY

CREATE TABLE NguoiChoThue
(
	IdNguoiChoThue  BIGINT IDENTITY(1,1),
	IdNguoiDung BIGINT NOT NULL,
	TenNguoiChoThue NVARCHAR(255) NOT NULL,
	NgaySinh DATE,
	GioiTinh NCHAR(5),
	DiaChi NVARCHAR(255),
	SoDienThoai CHAR(10),
	Email VARCHAR(100),
	CONSTRAINT PK_NhanVien PRIMARY KEY(IdNguoiChoThue),
	CONSTRAINT UNI_IdNguoiDung_NhanVien UNIQUE(IdNguoiDung),
	CONSTRAINT UNI_SoDienThoai_NhanVien UNIQUE(SoDienThoai),
	CONSTRAINT UNI_Email_NhanVien UNIQUE(Email),
	CONSTRAINT FK_NhanVien_NguoiDung FOREIGN KEY(IdNguoiDung) REFERENCES NguoiDung(IdNguoiDung)
);
ALTER TABLE NguoiChoThue


ADD CONSTRAINT DF_DiaChi_NguoiChoThue DEFAULT N'Không xác định' FOR DiaChi;


SET DATEFORMAT DMY
INSERT INTO NguoiChoThue (IdNguoiDung, TenNguoiChoThue, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email) VALUES
(4,N'Phạm Yến Nhi','16/04/2003',N'Nữ',N'Số 368, Quốc Kỷ, Hưng Thành, Vĩnh Lợi, Bạc Liêu, Việt Nam','0911051995','caynhalavuon@gmail.com'),
(5,N'Quảng Ling Ling','11/05/1995',N'Nữ',N'Tân Phú, Thành phố Hồ Chí Minh','090900099','00K@gmail.com')
--(3,N'Nguyễn Tấn Phát','07/04/2003',N'Nam',N'Tân Phú, Thành phố Hồ Chí Minh, Việt Nam','09090099','phatnguyen@gmail.com'),
--(4,N'Phạm Thị A','20/11/1990',N'Nữ',N'212-242 Đ. Độc Lập, Tân Thành, Tân Phú, Thành phố Hồ Chí Minh, Việt Nam','0393666222','co0giu20@gmail.com'),
--(5,N'Nguyễn Thị B','14/03/1998',N'Nữ',N'Bình Hưng Hòa A, Bình Hưng Hoà A, Bình Tân, Thành phố Hồ Chí Minh, Việt Nam','0393777222','matdungtim14@gmail.com'),
--(6,N'Lê Văn C','21/06/2000',N'Nam',N'Bình Tân, Thành phố Hồ Chí Minh, Việt Nam','0393000222','goi115ho@gmail.com')
SELECT* FROM NguoiChoThue

create table DiaChi
(
	IdDiaChi BIGINT IDENTITY(1,1), -- Mã định danh cho địa chỉ (Tự động tăng)
    SoNha NVARCHAR(255), -- Số nhà hoặc căn hộ
    Duong NVARCHAR(255), -- Tên đường
    Phuong NVARCHAR(255), -- Tên phường
    Quan NVARCHAR(255), -- Tên quận/huyện
    ThanhPho NVARCHAR(255) NOT NULL, -- Thành phố
    QuocGia NVARCHAR(255) NOT NULL DEFAULT N'Việt Nam', -- Quốc gia, mặc định là Việt Nam
    GhiChu NVARCHAR(255), -- Ghi chú thêm về địa chỉ nếu có
    CONSTRAINT PK_DiaChi PRIMARY KEY (IdDiaChi) -- Khóa chính
);
--================================================================================================================--
--================================================================================================================--
--================================================================================================================--
-- Quận 1
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Bến Nghé', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Bến Thành', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Cầu Ông Lãnh', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Cô Giang', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Đa Kao', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Nguyễn Cư Trinh', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Nguyễn Thái Bình', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Phạm Ngũ Lão', N'Quận 1', N'TP Hồ Chí Minh'),
    (N'Tân Định', N'Quận 1', N'TP Hồ Chí Minh');

-- Quận 2 (trong TP. Thủ Đức)
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'An Khánh', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'An Lợi Đông', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'An Phú', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Bình An', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Bình Khánh', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Bình Trưng Đông', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Bình Trưng Tây', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Cát Lái', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Thạnh Mỹ Lợi', N'Quận 2', N'TP Hồ Chí Minh'),
    (N'Thảo Điền', N'Quận 2', N'TP Hồ Chí Minh');

-- Quận 3
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 3', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 3', N'TP Hồ Chí Minh');

-- Quận 4
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 16', N'Quận 4', N'TP Hồ Chí Minh'),
    (N'Phường 18', N'Quận 4', N'TP Hồ Chí Minh');



-- Quận 5
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 5', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 5', N'TP Hồ Chí Minh');


-- Quận 6
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 6', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 6', N'TP Hồ Chí Minh');

-- Quận 7
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Tân Hưng', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Tân Kiểng', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Tân Phong', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Tân Quy', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Tân Thuận Đông', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Tân Thuận Tây', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Bình Thuận', N'Quận 7', N'TP Hồ Chí Minh'),
    (N'Phú Mỹ', N'Quận 7', N'TP Hồ Chí Minh');

-- Quận 8
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Quận 8', N'TP Hồ Chí Minh'),
    (N'Phường 16', N'Quận 8', N'TP Hồ Chí Minh');

-- Quận 10
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận 10', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Quận 10', N'TP Hồ Chí Minh');


-- Quận 12
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'An Phú Đông', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Đông Hưng Thuận', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Hiệp Thành', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Thạnh Lộc', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Thanh Xuân', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Thới An', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Tân Chánh Hiệp', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Tân Hưng Thuận', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Tân Thới Hiệp', N'Quận 12', N'TP Hồ Chí Minh'),
    (N'Tân Thới Nhất', N'Quận 12', N'TP Hồ Chí Minh');


-- Quận Bình Thạnh
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 17', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 19', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 21', N'Bình Thạnh', N'TP Hồ Chí Minh'),
    (N'Phường 22', N'Bình Thạnh', N'TP Hồ Chí Minh');



-- Quận Phú Nhuận
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Quận Phú Nhuận', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Quận Phú Nhuận', N'TP Hồ Chí Minh');

-- Quận Tân Phú
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phú Thọ Hòa', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Phú Thạnh', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Phú Trung', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Sơn Kỳ', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Tân Quý', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Tân Sơn Nhì', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Tân Thành', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Tân Thới Hòa', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Tây Thạnh', N'Tân Phú', N'TP Hồ Chí Minh'),
    (N'Hiệp Tân', N'Tân Phú', N'TP Hồ Chí Minh');

-- Quận Tân Bình
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Phường 1', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 2', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 3', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 4', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 5', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 6', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 7', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 8', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 9', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 10', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 11', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 12', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 13', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 14', N'Tân Bình', N'TP Hồ Chí Minh'),
    (N'Phường 15', N'Tân Bình', N'TP Hồ Chí Minh');

-- Quận Bình Tân
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'An Lạc', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'An Lạc A', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Hưng Hòa', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Hưng Hòa A', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Hưng Hòa B', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Trị Đông', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Trị Đông A', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Bình Trị Đông B', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Tân Tạo', N'Bình Tân', N'TP Hồ Chí Minh'),
    (N'Tân Tạo A', N'Bình Tân', N'TP Hồ Chí Minh');

-- Thủ Đức (thành phố Thủ Đức bao gồm các phường)
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Bình Chiểu', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Bình Thọ', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Hiệp Bình Chánh', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Hiệp Bình Phước', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Linh Chiểu', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Linh Đông', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Linh Tây', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Linh Trung', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Linh Xuân', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Tam Bình', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Tam Phú', N'Thủ Đức', N'TP Hồ Chí Minh'),
    (N'Trường Thọ', N'Thủ Đức', N'TP Hồ Chí Minh');

-- Huyện Bình Chánh
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Bình Chánh', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'An Phú Tây', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Bình Hưng', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Bình Lợi', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Đa Phước', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Hưng Long', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Phạm Văn Hai', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Phong Phú', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Quy Đức', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Tân Kiên', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Tân Nhựt', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Vĩnh Lộc A', N'Bình Chánh', N'TP Hồ Chí Minh'),
    (N'Vĩnh Lộc B', N'Bình Chánh', N'TP Hồ Chí Minh');

-- Huyện Củ Chi
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Thị trấn Củ Chi', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'An Nhơn Tây', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'An Phú', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Bình Mỹ', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Hòa Phú', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Nhuận Đức', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phạm Văn Cội', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phú Hòa Đông', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phú Mỹ Hưng', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phước Hiệp', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phước Thạnh', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phước Vĩnh An', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Tân An Hội', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Tân Phú Trung', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Tân Thạnh Đông', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Tân Thạnh Tây', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Tân Thông Hội', N'Củ Chi', N'TP Hồ Chí Minh'),
    (N'Phước Hiệp', N'Củ Chi', N'TP Hồ Chí Minh');

-- Huyện Hóc Môn
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Thị trấn Hóc Môn', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Bà Điểm', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Đông Thạnh', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Nhị Bình', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Tân Hiệp', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Tân Thới Nhì', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Thới Tam Thôn', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Xuân Thới Đông', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Xuân Thới Sơn', N'Hóc Môn', N'TP Hồ Chí Minh'),
    (N'Xuân Thới Thượng', N'Hóc Môn', N'TP Hồ Chí Minh');

-- Huyện Nhà Bè
INSERT INTO DiaChi (Phuong, Quan, ThanhPho) VALUES 
    (N'Thị trấn Nhà Bè', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Hiệp Phước', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Long Thới', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Nhơn Đức', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Phú Xuân', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Phước Kiển', N'Nhà Bè', N'TP Hồ Chí Minh'),
    (N'Phước Lộc', N'Nhà Bè', N'TP Hồ Chí Minh');
select * from DiaChi

--================================================================================================================--
--================================================================================================================--
--================================================================================================================--

CREATE TABLE DanhMuc
(
	IdDanhMuc INT IDENTITY(1,1),
	TenDanhMuc NVARCHAR(255),
	CONSTRAINT PK_danhmuc PRIMARY KEY(IdDanhMuc),
	CONSTRAINT UNI_TenDanhMuc UNIQUE(TenDanhMuc)
);

INSERT INTO DanhMuc (TenDanhMuc)
VALUES 
    (N'Chung Cư'),
    (N'Văn Phòng'),
    (N'Phòng Trọ Ghép'),
    (N'Căn Hộ Mini'),
    (N'Nhà Nguyên Căn'),
    (N'Mặt Bằng Kinh Doanh');

	select * from DanhMuc

create table PhongTro
(
    IdPhongTro BIGINT IDENTITY(1,1), -- Mã định danh cho phòng trọ (Tự động tăng)
    IdNguoiChoThue BIGINT NOT NULL, -- Liên kết với bảng NguoiChoThue
    IdDiaChi BIGINT NOT NULL, -- Liên kết với bảng DiaChi
    DienTich DECIMAL(5, 2) NOT NULL, -- Diện tích phòng trọ (đơn vị: m²)
    GiaPhong DECIMAL(10, 2) NOT NULL, -- Giá thuê phòng (đơn vị: VNĐ)
    MoTa NVARCHAR(1000), -- Mô tả về phòng trọ
    TrangThaiPhong NVARCHAR(50) NOT NULL DEFAULT N'con_trong', -- Trạng thái phòng (có thể là con_trong, da_thue, dang_bao_tri)
	IdDanhMuc INT not null,
    CONSTRAINT PK_PhongTro PRIMARY KEY (IdPhongTro), -- Khóa chính
    CONSTRAINT FK_PhongTro_NguoiChoThue FOREIGN KEY (IdNguoiChoThue) REFERENCES NguoiChoThue(IdNguoiChoThue), -- Khóa ngoại liên kết với bảng NguoiChoThue
    CONSTRAINT FK_PhongTro_DiaChi FOREIGN KEY (IdDiaChi) REFERENCES DiaChi(IdDiaChi), -- Khóa ngoại liên kết với bảng DiaChi
	CONSTRAINT FK_PhongTro_DanhMuc FOREIGN KEY (IdDanhMuc) REFERENCES DanhMuc(IdDanhMuc)
);
INSERT INTO PhongTro (IdNguoiChoThue, IdDiaChi, DienTich, GiaPhong, MoTa, TrangThaiPhong, IdDanhMuc)  VALUES 
    (1, 1, 25.5, 3500000, N'Phòng trọ đầy đủ nội thất', N'con_trong', 3),
    (2, 2, 40.0, 5000000, N'Căn hộ mini có view đẹp', N'da_thue', 4);

INSERT INTO PhongTro (IdNguoiChoThue, IdDiaChi, DienTich, GiaPhong, MoTa, TrangThaiPhong, IdDanhMuc)  VALUES 
(3, 176, 40.0, 5000000, N'Cho thuê văn phòng 5 tầng', N'con_trong', 2)
select * from PhongTro

create table BaiDang
(
    IdBaiDang BIGINT IDENTITY(1,1), -- Mã định danh cho bài đăng (tự động tăng)
    IdNguoiChoThue BIGINT NOT NULL, -- Liên kết với bảng NguoiChoThue
    IdPhongTro BIGINT NOT NULL, -- Liên kết với bảng PhongTro
    TieuDe NVARCHAR(255) NOT NULL, -- Tiêu đề bài đăng
    NoiDung NVARCHAR(2000) NOT NULL Default N'Nội dung trống', -- Nội dung mô tả chi tiết về phòng trọ
	AnhBaiDang NVARCHAR(500) not null default N'def_img.jpg',
    NgayDang DATE NOT NULL DEFAULT GETDATE(), -- Ngày đăng bài
    TrangThai NVARCHAR(50) NOT NULL DEFAULT 0, -- Trạng thái bài đăng (đang_hoat_dong= 0,  bi_an = 1)
	SoLuongYeuThich INT Default 0,
    CONSTRAINT PK_BaiDang PRIMARY KEY (IdBaiDang), -- Khóa chính
    CONSTRAINT FK_BaiDang_NguoiChoThue FOREIGN KEY (IdNguoiChoThue) REFERENCES NguoiChoThue(IdNguoiChoThue), -- Khóa ngoại liên kết với bảng NguoiChoThue
    CONSTRAINT FK_BaiDang_PhongTro FOREIGN KEY (IdPhongTro) REFERENCES PhongTro(IdPhongTro) -- Khóa ngoại liên kết với bảng PhongTro

);
INSERT INTO BaiDang (IdNguoiChoThue, IdPhongTro, TieuDe, NoiDung) VALUES 
    (1, 1, N'Cho thuê phòng trọ đầy đủ tiện nghi', N'Phòng rộng 25m², có sẵn nội thất, gần chợ và trường học'),
    (2, 2, N'Cho thuê căn hộ mini view đẹp', N'Căn hộ mini rộng 40m², phù hợp cho gia đình nhỏ');
 select * from BaiDang

CREATE TABLE AnhBaiDang
(
    IdAnh BIGINT IDENTITY(1,1), -- Mã định danh cho ảnh (tự động tăng)
    IdBaiDang BIGINT NOT NULL, -- Liên kết với bảng BaiDang
    DuongDanAnh NVARCHAR(500) NOT NULL, -- Đường dẫn hoặc URL của ảnh
    
    CONSTRAINT PK_AnhBaiDang PRIMARY KEY (IdAnh), -- Khóa chính
    CONSTRAINT FK_AnhBaiDang_BaiDang FOREIGN KEY (IdBaiDang) REFERENCES BaiDang(IdBaiDang) 
);
INSERT INTO AnhBaiDang  VALUES 
    (1, N'post_img1.jpg'),
    (1, N'post_img2.jpg'),
    (2, N'post_img3.jpg');	
select * from AnhBaiDang

CREATE TABLE PhanHoi (
    IdPhanHoi BIGINT IDENTITY(1,1), 
    IdNguoiThue BIGINT NOT NULL, 
    IdBaiDang BIGINT NOT NULL,
    NoiDung NVARCHAR(2000) NOT NULL, -- Nội dung phản hồi
    NgayPhanHoi DATE NOT NULL DEFAULT GETDATE(), -- Ngày phản hồi
    CONSTRAINT PK_PhanHoi PRIMARY KEY (IdPhanHoi), -- Khóa chính
    CONSTRAINT FK_PhanHoi_NguoiThue FOREIGN KEY (IdNguoiThue) REFERENCES NguoiThue(IdNguoiThue ), -- Khóa ngoại liên kết với bảng NguoiDung
    CONSTRAINT FK_PhanHoi_BaiDang FOREIGN KEY (IdBaiDang) REFERENCES BaiDang(IdBaiDang) -- Khóa ngoại liên kết với bảng BaiDang
);
INSERT INTO PhanHoi (IdNguoiThue, IdBaiDang, NoiDung) VALUES 
    (1, 1, N'Phòng rất đẹp, tôi rất thích!'),
    (1, 2, N'Thích hợp cho gia đình nhỏ, không gian yên tĩnh.');
select * from PhanHoi


create table YeuThich(
	IDYT INT IDENTITY(1,1), 
	IdBaiDang  BIGINT NOT NULL,
	IdNguoiDung  BIGINT NOT NULL,
	CONSTRAINT PK_YeuThich PRIMARY KEY (IDYT),
    CONSTRAINT FK_YeuThich_NguoiDung FOREIGN KEY (IdNguoiDung) REFERENCES NguoiDUng(IdNguoiDung ), -- Khóa ngoại liên kết với bảng NguoiDung
    CONSTRAINT FK_YeuThich_BaiDang FOREIGN KEY (IdBaiDang) REFERENCES BaiDang(IdBaiDang) -- Khóa ngoại liên kết với bảng BaiDang
)
 INSERT INTO YeuThich(IdBaiDang,IdNguoiDung)  VALUES 
    (1,7),
    (2,6)	
select * from YeuThich

create table kiemduyetbaidang(
	IDKDB INT IDENTITY(1,1), 
	IdBaiDang  BIGINT NOT NULL,
	IdKDV BIGINT NOT NULL,
	NoiDung nvarchar(255),
	ngayduyet datetime,
	PRIMARY KEY(IDKDB),
    CONSTRAINT FK_KDBD_BaiDang FOREIGN KEY (IdBaiDang) REFERENCES BaiDang(IdBaiDang),
    CONSTRAINT FK_KDBD_KDV FOREIGN KEY (IdKDV) REFERENCES kiemduyetvien(IdKDV) 
)



CREATE FUNCTION func_(@Id bigint)
RETURNS BIGINT
AS
BEGIN
	DECLARE @Gia BIGINT;
    SELECT @Gia = GiaBan - GiaBan * GiamGia /100
	FROM SanPham WHERE IdSanPham = @IdSanPham
    RETURN ISNULL(@Gia, 0);
END;