namespace KhaoThiSoftware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Table_Name_Roles : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Role", newName: "Roles");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Roles", newName: "Role");
        }
    }
}
