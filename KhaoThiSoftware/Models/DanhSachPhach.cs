namespace KhaoThiSoftware.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DanhSachPhachs")]
    public partial class DanhSachPhach
    {
        [Key]
        public long IdDanhSachPhach { get; set; }
        public string SoPhach { get; set; }
        public double IdDanhSachThi { get; set; }
        public int IdKyThi { get; set; }
    }
}
