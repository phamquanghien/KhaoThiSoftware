namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_KetQuaThi_TestNhapDiem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KetQuaThis",
                c => new
                    {
                        IdKetQuaThi = c.Long(nullable: false, identity: true),
                        SoPhach = c.String(),
                        Diem1 = c.Single(nullable: false),
                        Diem2 = c.Single(nullable: false),
                        DiemTrungBinh = c.Single(nullable: false),
                        IdKyThi = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdKetQuaThi);
            
            CreateTable(
                "dbo.TestNhapDiems",
                c => new
                    {
                        IdKetQuaThi = c.Long(nullable: false, identity: true),
                        SoPhach = c.String(),
                        Diem1 = c.Single(nullable: false),
                        Diem2 = c.Single(nullable: false),
                        DiemTrungBinh = c.Single(nullable: false),
                        IdKyThi = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdKetQuaThi);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestNhapDiems");
            DropTable("dbo.KetQuaThis");
        }
    }
}
