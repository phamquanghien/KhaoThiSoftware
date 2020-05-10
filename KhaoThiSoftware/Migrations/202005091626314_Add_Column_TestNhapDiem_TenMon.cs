namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Column_TestNhapDiem_TenMon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestNhapDiems", "f_tenmhvn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestNhapDiems", "f_tenmhvn");
        }
    }
}
