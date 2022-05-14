namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThanhVien")]
    public partial class ThanhVien
    {
        [Key]
        public int MaTV { get; set; }

        [StringLength(255)]
        public string HoTen { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public int? UuDai { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        public bool? isAdmin { get; set; }

        public int? Solandatphongthanhcong { get; set; }
    }
}
