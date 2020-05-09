using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KhaoThiSoftware.Models
{
    [Table("ThongKePhachs")]
    public class ThongKePhach
    {
        [Key]
        public long IdThongKePhach { get; set; }
        public string TenMonThi { get; set; }
        public int PhachBatDau { get; set; }
        public int PhachKetThuc { get; set; }
        public int? IdKyThi { get; set; }
    }
}