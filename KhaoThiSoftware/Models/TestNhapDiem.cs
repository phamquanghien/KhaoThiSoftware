using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KhaoThiSoftware.Models
{
    [Table("TestNhapDiems")]
    public class TestNhapDiem
    {
        [Key]
        public long IdKetQuaThi { get; set; }
        public string SoPhach { get; set; }
        public float Diem1 { get; set; }
        public float Diem2 { get; set; }
        public float DiemTrungBinh { get; set; }
        public int IdKyThi { get; set; }
        public bool Active { get; set; }
    }
}