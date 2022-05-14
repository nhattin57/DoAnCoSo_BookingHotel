namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DatPhong")]
    public partial class DatPhong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DatPhong()
        {
            ChiTietDatPhongs = new HashSet<ChiTietDatPhong>();
        }

        [Key]
        public int MaDatPhong { get; set; }

        public int? MaPhong { get; set; }

        public int? MaKH { get; set; }

        public DateTime? NgayDat { get; set; }

        public DateTime? NgayTra { get; set; }

        public bool? DaXoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDatPhong> ChiTietDatPhongs { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
