namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checks", "IPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Checks", "IPAddress");
        }
    }
}
