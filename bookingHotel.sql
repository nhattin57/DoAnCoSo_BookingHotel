Create database BookingHotel
use BookingHotel
---

create table Phong(
	MaPhong int identity(1,1),
	TenPhong  nvarchar(255),	
	Don_Gia	decimal,
	SoNguoiLon	int,
	SoTreEm int,
	Mota nvarchar(MAX),
	HinhAnh1 nvarchar(256),
	HinhAnh2 nvarchar(256),	
	HinhAnh3 nvarchar(256),
	HinhAnh4 nvarchar(256),
	HinhAnh5 nvarchar(256),
	HinhAnh6 nvarchar(256),
	MaLoaiPhong int,
	DaXoa bit
	primary key(MaPhong)
)

--
create table LoaiPhong(
MaLoaiPhong int identity(1,1),
TenLoaiPhong nvarchar(255)
primary key(MaLoaiPhong)
)

--
create table KhachHang(
MaKH int identity(1,1),
HoTen nvarchar(255),
SDT varchar(15),
Email varchar(256)
primary key(MaKH)
)
--
create table ThanhVien(
MaTV int identity(1,1),
HoTen nvarchar(255),
SDT varchar(15),
Email varchar(256),
UuDai int,
MatKhau nvarchar(50),
isAdmin bit,
Solandatphongthanhcong int,
primary key(MaTV)
)

--

create table DatPhong(
MaDatPhong int identity(1,1),
MaPhong int,
MaKH int,
NgayDat datetime,
NgayTra datetime,
GhiChu nvarchar(256),
DaXoa bit,
primary key(MaDatPhong)
)
--
create table ChiTietDatPhong(
MaDatPhong int,
MaKH int,
DonGia_Dat decimal,
primary key(MaDatPhong,MaKH)
)
--

Create table BinhLuan(
MaPhong int,
MaKH int,
NoiDungBinhLuan nvarchar(MAX),
ThoiGianBL datetime
primary key(MaPhong,MaKH)
)
--
create table PhanHoiTuKH(
MaPhieuPH int identity(1,1),
HoTen nvarchar(128),
Email varchar(128),
TieuDe nvarchar(128),
NoiDung nvarchar(MAX)
primary key(MaPhieuPH)
)
-------------tao khoa ngoai
alter table Phong add foreign key (MaLoaiPhong) references LoaiPhong(MaLoaiPhong) on delete no action
alter table DatPhong add foreign key (MaPhong) references Phong(MaPhong) on delete no action
alter table DatPhong add foreign key (MaKH) references KhachHang(MaKH) on delete no action
alter table ChiTietDatPhong add foreign key (MaDatPhong) references DatPhong(MaDatPhong) on delete no action
alter table ChiTietDatPhong add foreign key (MaKH) references KhachHang(MaKH) on delete no action
alter table BinhLuan add foreign key (MaPhong) references Phong(MaPhong) on delete no action
alter table BinhLuan add foreign key (MaKH) references KhachHang(MaKH) on delete no action
---

--them du lieu
insert into LoaiPhong values(N'Phòng đơn (Tối đa 2 người lớn)')
insert into LoaiPhong values(N'Phòng Đôi (Tối đa 5 người lớn)')

insert into Phong values(N'Phòng Đơn Lexure',1000000,2,1,
N'Phòng Deluxe được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 30 – 50m vuông. Không gian rộng rãi, sang trọng của phòng Deluxe 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
N'don_lexure1.jpg','don_lexure2.jpg','don_lexure3.jpg','don_lexure4.jpg',
'don_lexure5.jpg','don_lexure6.jpg',1,0)

insert into Phong values(N'Phòng Đơn Superior',600000,2,1,
N'Phòng Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 20 – 40m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
N'don_superior1.jpg','don_superior2.jpg','don_superior3.jpg','don_superior4.jpg',
'don_superior5.jpg','don_superior6.jpg',1,0)

insert into Phong values(N'Phòng Đôi Superior',800000,5,2,
N'Phòng Đôi Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 30 – 60m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
'doi_superior1.jpg','doi_superior2.jpg','doi_superior3.jpg','don_lexure5.jpg',
'don_lexure3.jpg','don_lexure6.jpg',2,0)

insert into Phong values(N'Phòng Đôi Lexure',1500000,5,2,
N'Phòng Đôi Lexure được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 30 – 60m vuông. Không gian rộng rãi, sang trọng của phòng Lexure 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
'doi_lexure1.jpg','doi_lexure2.jpg','doi_lexure3.jpg','doi_lexure4.jpg',
'don_lexure5.jpg','don_lexure6.jpg',2,0)

insert into Phong values(N'Phòng Thường',400000,2,1,
N'Phòng Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 20 – 30m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, cặp vợ chồng có con nhỏ…',
N'don_superior1.jpg','don_superior2.jpg','don_superior3.jpg','don_superior4.jpg',
'don_superior5.jpg','don_superior6.jpg',1,0)

insert into Phong values(N'Phòng Thường',500000,2,1,
N'Phòng Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 20 – 30m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, cặp vợ chồng có con nhỏ…',
N'don_superior1.jpg','don_superior2.jpg','don_superior3.jpg','don_superior4.jpg',
'don_superior5.jpg','don_superior6.jpg',1,0)

insert into Phong values(N'Phòng Đôi Thường',600000,4,2,
N'Phòng Đôi Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 30 – 60m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
'doi_superior1.jpg','doi_superior2.jpg','doi_superior3.jpg','don_lexure5.jpg',
'don_lexure3.jpg','don_lexure6.jpg',2,0)

insert into Phong values(N'Phòng Đôi Thường',500000,4,2,
N'Phòng Đôi Superior được thiết kế với diện tích rộng, ở tầng cao, 
có hướng nhìn đẹp và được trang bị những vật dụng, trang thiết bị cao cấp. 
Có diện tích từ 30 – 60m vuông. Không gian rộng rãi, sang trọng của phòng Superior 
thích hợp với nhiều đối tượng khách hàng khác nhau: Du lịch nghỉ dưỡng, công tác, cặp vợ chồng có con nhỏ…',
'doi_superior1.jpg','doi_superior2.jpg','doi_superior3.jpg','don_lexure5.jpg',
'don_lexure3.jpg','don_lexure6.jpg',2,0)

insert into ThanhVien values(N'Đào Nhật Tín','0384480806','nhattin57@gmail.com',5,'nhattin',0,0)

-- sửa thì chạy chỗ này

alter table DatPhong add GhiChu nvarchar(256)

alter table ChiTietDatPhong drop column GhiChu