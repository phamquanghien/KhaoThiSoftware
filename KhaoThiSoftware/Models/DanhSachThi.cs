namespace KhaoThiSoftware.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DanhSachThi
    {
        public int Id { get; set; }

        public string f_masv { get; set; }

        public string f_mamh { get; set; }

        public string f_holotvn { get; set; }

        public string f_tenvn { get; set; }

        public string f_ngaysinh { get; set; }

        public int sobaodanh { get; set; }

        public string f_tenlop { get; set; }

        public string f_tenmhvn { get; set; }

        public DateTime ngaythi { get; set; }

        public string phongthi { get; set; }

        public int tietbatdau { get; set; }

        public int sotiet { get; set; }
    }
}
