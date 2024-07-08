namespace HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        CheckInId = c.Int(nullable: false),
                        Country = c.String(),
                        Region = c.String(),
                        City = c.String(),
                        Zip = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Employee_Emp_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Employees", t => t.Employee_Emp_id)
                .ForeignKey("dbo.Checks", t => t.CheckInId, cascadeDelete: true)
                .Index(t => t.CheckInId)
                .Index(t => t.Employee_Emp_id);
            
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emp_id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Work_Hours = c.Single(nullable: false),
                        Break_Hours = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Emp_id, cascadeDelete: true)
                .Index(t => t.Emp_id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Emp_id = c.String(nullable: false, maxLength: 128),
                        First_Name = c.String(),
                        Middle_Name = c.String(),
                        Last_Name = c.String(),
                        Emp_Image = c.String(),
                        Father_s_Husband_s_Name = c.String(),
                        Gender = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Marital_Status = c.String(),
                        Religions = c.String(),
                        Nationality = c.String(),
                        Blood_Group = c.String(),
                    })
                .PrimaryKey(t => t.Emp_id);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emp_id = c.String(nullable: false, maxLength: 128),
                        CheckTime = c.DateTime(nullable: false),
                        CheckType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Emp_id, cascadeDelete: true)
                .Index(t => t.Emp_id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Bank_id = c.String(nullable: false, maxLength: 128),
                        Emp_Id = c.String(maxLength: 128),
                        Bank_Name = c.String(),
                        Branch_Name = c.String(),
                        Branch_Code = c.String(),
                        Account_Title = c.String(),
                        Account_No = c.String(),
                        Effective_Date = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Bank_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Contact_id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Email = c.String(),
                        Land_Line = c.String(),
                        Mobile_No = c.String(),
                    })
                .PrimaryKey(t => t.Contact_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Education_id = c.Long(nullable: false, identity: true),
                        Emp_id = c.String(maxLength: 128),
                        University_College_Name = c.String(),
                        Degree = c.String(),
                        From_Date = c.DateTime(nullable: false),
                        To_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Education_id)
                .ForeignKey("dbo.Employees", t => t.Emp_id)
                .Index(t => t.Emp_id);
            
            CreateTable(
                "dbo.Emp_Documents",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Document_Type_id = c.Long(nullable: false),
                        Name = c.String(),
                        File = c.String(),
                        Issue_Date = c.String(),
                        Emp_Document_Type_EDT_id = c.Long(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Emp_Document_Type", t => t.Emp_Document_Type_EDT_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id)
                .Index(t => t.Emp_Document_Type_EDT_id);
            
            CreateTable(
                "dbo.Emp_Document_Type",
                c => new
                    {
                        EDT_id = c.Long(nullable: false, identity: true),
                        Docuement_Type = c.String(),
                    })
                .PrimaryKey(t => t.EDT_id);
            
            CreateTable(
                "dbo.Employee_info",
                c => new
                    {
                        Emp_info_id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Joining_date = c.DateTime(nullable: false),
                        Probation = c.Boolean(nullable: false),
                        Probation_Confirmation_Date = c.DateTime(),
                        Location = c.String(),
                        Department_Id = c.Long(nullable: false),
                        Branch_id = c.Long(nullable: false),
                        Designation = c.String(),
                        Job_Title = c.String(),
                        Official_Email = c.String(),
                        Employment_Type = c.String(),
                        Machine_Code = c.String(),
                    })
                .PrimaryKey(t => t.Emp_info_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Experiences",
                c => new
                    {
                        Exp_id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Company_name = c.String(),
                        Position = c.String(),
                        From_Date = c.DateTime(nullable: false),
                        To_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Exp_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        Login_ID = c.Int(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Email = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        VerificationToken = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        ResetPasswordCode = c.String(),
                        ResetPasswordExpiry = c.String(),
                    })
                .PrimaryKey(t => t.Login_ID)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Relatives",
                c => new
                    {
                        Relative_id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        Name = c.String(),
                        Relationship = c.String(),
                        Address = c.String(),
                        Mobile_No = c.String(),
                        Contact_Type = c.String(),
                    })
                .PrimaryKey(t => t.Relative_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        Sal_id = c.Long(nullable: false, identity: true),
                        Emp_id = c.String(maxLength: 128),
                        Payroll_Type = c.Long(nullable: false),
                        Frequency = c.String(),
                        NTN_Number = c.String(),
                        Payment_Method = c.String(),
                        Probation_Salary = c.Double(nullable: false),
                        After_Probation_Gross_Salary = c.Double(nullable: false),
                        Tax_Exempted_ = c.Boolean(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Sal_id)
                .ForeignKey("dbo.Employees", t => t.Emp_id)
                .Index(t => t.Emp_id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchId = c.Int(nullable: false, identity: true),
                        BranchName = c.String(),
                    })
                .PrimaryKey(t => t.BranchId);
            
            CreateTable(
                "dbo.CNICs",
                c => new
                    {
                        Document_id = c.Long(nullable: false, identity: true),
                        Emp_Id = c.String(maxLength: 128),
                        CNIC_Number = c.String(),
                        Document_Attachment = c.String(),
                        Document_Expiry = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Document_id)
                .ForeignKey("dbo.Employees", t => t.Emp_Id)
                .Index(t => t.Emp_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CNICs", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Addresses", "CheckInId", "dbo.Checks");
            DropForeignKey("dbo.Checks", "Emp_id", "dbo.Employees");
            DropForeignKey("dbo.Salaries", "Emp_id", "dbo.Employees");
            DropForeignKey("dbo.Relatives", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Logins", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Experiences", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Employee_info", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Emp_Documents", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Emp_Documents", "Emp_Document_Type_EDT_id", "dbo.Emp_Document_Type");
            DropForeignKey("dbo.Educations", "Emp_id", "dbo.Employees");
            DropForeignKey("dbo.Contacts", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Banks", "Emp_Id", "dbo.Employees");
            DropForeignKey("dbo.Attendances", "Emp_id", "dbo.Employees");
            DropForeignKey("dbo.Addresses", "Employee_Emp_id", "dbo.Employees");
            DropIndex("dbo.CNICs", new[] { "Emp_Id" });
            DropIndex("dbo.Salaries", new[] { "Emp_id" });
            DropIndex("dbo.Relatives", new[] { "Emp_Id" });
            DropIndex("dbo.Logins", new[] { "Emp_Id" });
            DropIndex("dbo.Experiences", new[] { "Emp_Id" });
            DropIndex("dbo.Employee_info", new[] { "Emp_Id" });
            DropIndex("dbo.Emp_Documents", new[] { "Emp_Document_Type_EDT_id" });
            DropIndex("dbo.Emp_Documents", new[] { "Emp_Id" });
            DropIndex("dbo.Educations", new[] { "Emp_id" });
            DropIndex("dbo.Contacts", new[] { "Emp_Id" });
            DropIndex("dbo.Banks", new[] { "Emp_Id" });
            DropIndex("dbo.Attendances", new[] { "Emp_id" });
            DropIndex("dbo.Checks", new[] { "Emp_id" });
            DropIndex("dbo.Addresses", new[] { "Employee_Emp_id" });
            DropIndex("dbo.Addresses", new[] { "CheckInId" });
            DropTable("dbo.Departments");
            DropTable("dbo.CNICs");
            DropTable("dbo.Branches");
            DropTable("dbo.Salaries");
            DropTable("dbo.Relatives");
            DropTable("dbo.Logins");
            DropTable("dbo.Experiences");
            DropTable("dbo.Employee_info");
            DropTable("dbo.Emp_Document_Type");
            DropTable("dbo.Emp_Documents");
            DropTable("dbo.Educations");
            DropTable("dbo.Contacts");
            DropTable("dbo.Banks");
            DropTable("dbo.Attendances");
            DropTable("dbo.Employees");
            DropTable("dbo.Checks");
            DropTable("dbo.Addresses");
        }
    }
}
