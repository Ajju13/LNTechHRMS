namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "Middle_Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Middle_Name", c => c.String());
        }
    }
}
