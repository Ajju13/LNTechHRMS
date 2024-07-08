using HRMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class EmployeeViewModel
    {
        public string BankName { get; set; }
        public string AccountTitle { get; set; }
        public string Emp_Id { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string Emp_Image { get; set; }
        public string Emp_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }

        public List<Bank> Bank { get; set; }
        public string Phone { get; set; }

        public DateTime JoinDate { get; set; }

        public string JobTitle {  get; set; }

        public DateTime EffectiveDate { get; set; }  

    }



    public class AddEmployeeViewModel
    {
        public string Emp_Image_Full_Url { get; set; }

        public string Emp_Id { get; set; }

        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        public string FatherHusbandName { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Marital_Status { get; set; }
        public string Emp_Image { get; set; }
        public string Religions { get; set; }
        public string Nationality { get; set; }
        public string Blood_Group { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime Joining_date { get; set; }
        public string JobTitle { get; set; }
        public string email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string phone { get; set; }
        
        public string landline { get; set; }
        public bool Probation { get; set;}

        [Display(Name = "Probation Confirmation Date")]
        [DataType(DataType.Date)]
        public DateTime? Probation_Confirmation_Date { get; set; }

        public string Location { get; set; }
        public long DepatmentId { get; set; }
        public long Branch_id { get; set; }
        public string Designation { get; set; }
        public string Official_Email { get; set; }
        public string Employment_Type { get; set; }
        public string Machine_Code { get; set; }


    }

    public class ViewEmployeePageModel
    {


        public IEnumerable<EmployeeViewModel> Employees { get; set; }
        public AddEmployeeViewModel AddEmployee { get; set; }
        public List<Department> Departments { get; set; }
        public List<Branch> Branches { get; set; } 

        public EditEmployeeViewModel EditEmployee { get; set; }


        public List<Bank> Bank { get; set; }
    }

    public class EditEmployeeViewModel
    {
        public string Emp_Id { get; set; }

        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        public string FatherHusbandName { get; set; }
        public string Gender { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public string Marital_Status { get; set; }
        public string Emp_Image { get; set; }
        public string Religions { get; set; }
        public string Nationality { get; set; }
        public string Blood_Group { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime Joining_date { get; set; }
        public string JobTitle { get; set; }
        public string email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string phone { get; set; }

        public string landline { get; set; }
        public bool Probation { get; set; }

        [Display(Name = "Probation Confirmation Date")]
        [DataType(DataType.Date)]
        public DateTime? Probation_Confirmation_Date { get; set; }

        public string Location { get; set; }
        public long DepatmentId { get; set; }
        public long Branch_id { get; set; }
        public string Designation { get; set; }
        public string Official_Email { get; set; }
        public string Employment_Type { get; set; }
        public string Machine_Code { get; set; }
    }
}