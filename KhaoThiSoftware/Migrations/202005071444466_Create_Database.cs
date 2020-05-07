namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, unicode: false),
                        RoleID = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.DanhSachPhachs",
                c => new
                    {
                        IdDanhSachPhach = c.Long(nullable: false, identity: true),
                        SoPhach = c.String(),
                        IdDanhSachThi = c.Long(nullable: false),
                        IdKyThi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDanhSachPhach);
            
            CreateTable(
                "dbo.DanhSachThis",
                c => new
                    {
                        IdDanhSachThi = c.Long(nullable: false, identity: true),
                        f_masv = c.String(),
                        f_mamh = c.String(),
                        f_holotvn = c.String(),
                        f_tenvn = c.String(),
                        f_ngaysinh = c.String(),
                        sobaodanh = c.Int(),
                        f_tenlop = c.String(),
                        f_tenmhvn = c.String(),
                        ngaythi = c.DateTime(),
                        phongthi = c.String(),
                        tietbatdau = c.Byte(),
                        sotiet = c.Byte(),
                        IdKyThi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDanhSachThi);
            
            CreateTable(
                "dbo.KyThis",
                c => new
                    {
                        IdKyThi = c.Int(nullable: false, identity: true),
                        MaKyThi = c.String(nullable: false, unicode: false),
                        TenKyThi = c.String(nullable: false),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(),
                        ChuThich = c.String(),
                        isDelete = c.Boolean(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdKyThi);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 10, unicode: false),
                        RoleName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Roles");
            DropTable("dbo.KyThis");
            DropTable("dbo.DanhSachThis");
            DropTable("dbo.DanhSachPhachs");
            DropTable("dbo.Accounts");
        }
    }
}
