namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Table_ThongKePhach : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ThongKePhachs", "IdKyThi", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThongKePhachs", "IdKyThi", c => c.Int(nullable: false));
        }
    }
}
