using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KhaoThiSoftware.Models
{
    [Table("Roles")]
    public partial class Role
    {
        [Key]
        [StringLength(10)]
        public string RoleID { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
