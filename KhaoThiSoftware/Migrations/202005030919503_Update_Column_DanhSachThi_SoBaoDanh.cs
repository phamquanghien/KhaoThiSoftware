namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Column_DanhSachThi_SoBaoDanh : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DanhSachThis", "sobaodanh", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DanhSachThis", "sobaodanh", c => c.Int(nullable: false));
        }
    }
}
