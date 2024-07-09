namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalaryRevisions",
                c => new
                    {
                        SalReviseId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(nullable: false, maxLength: 128),
                        ReviseSalary = c.Double(nullable: false),
                        EffectiveDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalReviseId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalaryRevisions", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.SalaryRevisions", new[] { "EmployeeId" });
            DropTable("dbo.SalaryRevisions");
        }
    }
}
