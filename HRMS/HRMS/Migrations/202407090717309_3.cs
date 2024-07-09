namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Salaries", "Payroll_Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salaries", "Payroll_Type", c => c.Long(nullable: false));
        }
    }
}
