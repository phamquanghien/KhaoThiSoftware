namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_ThongKePhach : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThongKePhachs",
                c => new
                    {
                        IdThongKePhach = c.Long(nullable: false, identity: true),
                        TenMonThi = c.String(),
                        PhachBatDau = c.Int(nullable: false),
                        PhachKetThuc = c.Int(nullable: false),
                        IdKyThi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdThongKePhach);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ThongKePhachs");
        }
    }
}
