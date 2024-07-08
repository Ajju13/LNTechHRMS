using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml;
using HRMS.Functions;

namespace HRMS.Models
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext()
        : base("name=DefaultConnection")
        {
        }
        public DbSet<Emp_Document_Type> EmpDocumentTypes { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Relative> Relatives { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Emp_Documents> EmpDocuments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<CNIC> CNIC { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee_info> EmployeeInfos { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Branch> Branch { get; set; }

        public DbSet<Check> Check { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet<Attendances> Attendances { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Check>()
                .ToTable("Checks")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Attendances>()
                .ToTable("Attendances")
                .HasKey(a => a.Id);
        }

        public override int SaveChanges()
        {
            var modifiedCheckEntities = ChangeTracker.Entries<Check>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .Select(e => e.Entity)
                .ToList();

            int result = base.SaveChanges();

            if (modifiedCheckEntities.Any())
            {
                var attendanceService = new AttendanceService(this);
                attendanceService.UpdateAttendanceForChecks(modifiedCheckEntities);
            }

            return result;
        }
    }

}