using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhaoThiSoftware.Models
{
    [Table("KetQuaThis")]
    public class KetQuaThi
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