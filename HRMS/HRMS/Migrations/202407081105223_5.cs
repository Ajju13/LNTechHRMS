namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checks", "CheckTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Checks", "CheckType", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Attendances", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Attendances", "Work_Hours", c => c.Single(nullable: false));
            AddColumn("dbo.Attendances", "Break_Hours", c => c.Single(nullable: false));
            DropColumn("dbo.Checks", "Date");
            DropColumn("dbo.Checks", "Work_Hours");
            DropColumn("dbo.Checks", "Break_Hours");
            DropColumn("dbo.Attendances", "CheckTime");
            DropColumn("dbo.Attendances", "CheckType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attendances", "CheckType", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Attendances", "CheckTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Checks", "Break_Hours", c => c.Single(nullable: false));
            AddColumn("dbo.Checks", "Work_Hours", c => c.Single(nullable: false));
            AddColumn("dbo.Checks", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Attendances", "Break_Hours");
            DropColumn("dbo.Attendances", "Work_Hours");
            DropColumn("dbo.Attendances", "Date");
            DropColumn("dbo.Checks", "CheckType");
            DropColumn("dbo.Checks", "CheckTime");
        }
    }
}
