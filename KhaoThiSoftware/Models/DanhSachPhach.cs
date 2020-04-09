namespace KhaoThiSoftware.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhSachPhachs")]
    public partial class DanhSachPhach
    {
        public long Id { get; set; }

        public string f_mamh { get; set; }

        public string SoPhach { get; set; }
    }
}
