namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhanHoiTuKH")]
    public partial class PhanHoiTuKH
    {
        [Key]
        public int MaPhieuPH { get; set; }

        [StringLength(128)]
        public string HoTen { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(128)]
        public string TieuDe { get; set; }

        public string NoiDung { get; set; }
    }
}
