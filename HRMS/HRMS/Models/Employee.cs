using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HRMS.Models
{
    public class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Addresses = new HashSet<Address>();
            this.Banks = new HashSet<Bank>();
            this.Contacts = new HashSet<Contact>();
            this.Educations = new HashSet<Education>();
            this.Emp_Documents = new HashSet<Emp_Documents>();
            this.Employee_info = new HashSet<Employee_info>();
            this.Experiences = new HashSet<Experience>();
            this.Logins = new HashSet<Login>();
            this.Relatives = new HashSet<Relative>();
            this.Salaries = new HashSet<Salary>();
            this.Attendances = new HashSet<Attendances>();
        }
        [Key]
        public string Emp_id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Emp_Image { get; set; }
        public string Father_s_Husband_s_Name { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Marital_Status { get; set; }
        public string Religions { get; set; }
        public string Nationality { get; set; }
        public string Blood_Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bank> Banks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Education> Educations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Emp_Documents> Emp_Documents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee_info> Employee_info { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Experience> Experiences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Login> Logins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Relative> Relatives { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Attendances> Attendances { get; set; }

        public virtual ICollection<Check> Check { get; set; }

        public virtual ICollection<SalaryRevision> SalaryRevision { get; set; }
    }
}