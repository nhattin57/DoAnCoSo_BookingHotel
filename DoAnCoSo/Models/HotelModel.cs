using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAnCoSo.Models
{
    public partial class HotelModel : DbContext
    {
        public HotelModel()
            : base("name=HotelModel")
        {
        }

        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChiTietDatPhong> ChiTietDatPhongs { get; set; }
        public virtual DbSet<DatPhong> DatPhongs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public virtual DbSet<PhanHoiTuKH> PhanHoiTuKHs { get; set; }
        public virtual DbSet<Phong> Phongs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ThanhVien> ThanhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDatPhong>()
                .Property(e => e.DonGia_Dat)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DatPhong>()
                .HasMany(e => e.ChiTietDatPhongs)
                .WithRequired(e => e.DatPhong)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.BinhLuans)
                .WithRequired(e => e.KhachHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.ChiTietDatPhongs)
                .WithRequired(e => e.KhachHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhanHoiTuKH>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Phong>()
                .Property(e => e.Don_Gia)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Phong>()
                .HasMany(e => e.BinhLuans)
                .WithRequired(e => e.Phong)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<ThanhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
