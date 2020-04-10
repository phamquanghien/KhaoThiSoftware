namespace KhaoThiSoftware.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("KyThis")]
    public partial class KyThi
    {
        [Key]
        public int IdKyThi { get; set; }

        [Required]
        public string MaKyThi { get; set; }

        [Required]
        public string TenKyThi { get; set; }

        public DateTime NgayTao { get; set; }

        public string NguoiTao { get; set; }

        public string ChuThich { get; set; }

        public bool? isDelete { get; set; }

        public bool? Status { get; set; }
    }
}
