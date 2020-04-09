namespace KhaoThiSoftware.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        [StringLength(10)]
        public string RoleID { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
