namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDatPhong")]
    public partial class ChiTietDatPhong
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDatPhong { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaKH { get; set; }

        public decimal? DonGia_Dat { get; set; }

        [StringLength(256)]
        public string GhiChu { get; set; }

        public virtual DatPhong DatPhong { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
