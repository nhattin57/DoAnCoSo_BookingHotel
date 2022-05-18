namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Phong")]
    public partial class Phong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phong()
        {
            BinhLuans = new HashSet<BinhLuan>();
            DatPhongs = new HashSet<DatPhong>();
        }

        [Key]
        public int MaPhong { get; set; }

        [StringLength(255)]
        [Required]
        [Display(Name = "Tên Phòng")]
        public string TenPhong { get; set; }

        [Required]
        [Display(Name = "Giá Phòng")]
        public decimal? Don_Gia { get; set; }
        [Required]
        [Display(Name = "Số người lớn")]
        public int? SoNguoiLon { get; set; }
        [Required]
        [Display(Name = "Số trẻ em")]
        public int? SoTreEm { get; set; }
        [Required]
        [Display(Name = "Mô tả")]
        public string Mota { get; set; }


        [StringLength(256)]
        [Display(Name = "Hình ảnh 1")]
        public string HinhAnh1 { get; set; }


        [StringLength(256)]
        [Display(Name = "Hình ảnh 2")]
        public string HinhAnh2 { get; set; }

        [StringLength(256)]

        [Display(Name = "Hình ảnh 3")]

        public string HinhAnh3 { get; set; }


        [StringLength(256)]
        [Display(Name = "Hình ảnh 4")]

        public string HinhAnh4 { get; set; }


        [StringLength(256)]
        [Display(Name = "Hình ảnh 5")]

        public string HinhAnh5 { get; set; }

        [StringLength(256)]
        [Display(Name = "Hình ảnh 6")]

        public string HinhAnh6 { get; set; }

        [Display(Name = "Loại phòng")]
        public int? MaLoaiPhong { get; set; }

        public bool? DaXoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatPhong> DatPhongs { get; set; }

        public virtual LoaiPhong LoaiPhong { get; set; }
    }
}
