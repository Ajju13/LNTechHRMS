using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.ViewModels
{
    public class PersonalProfileViewModel
    {
        public string Emp_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string FatherHusbandName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Marital_Status { get; set; }
        public string Emp_Image { get; set; }
        public string Religions { get; set; }
        public string Nationality { get; set; }
        public string Blood_Group { get; set; }
        public DateTime Joining_date { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Landline { get; set; }
        public bool Probation { get; set; }
        public DateTime? Probation_Confirmation_Date { get; set; }
        public string Location { get; set; }
        public long DepartmentId { get; set; }
        public long BranchId { get; set; }
        public string Designation { get; set; }
        public string Official_Email { get; set; }
        public string Employment_Type { get; set; }
        public string Machine_Code { get; set; }
        public string Username { get; set; }

        public string CNICNo { get; set; }

        public DateTime CNICExpiry { get; set; }

        public string RelatvePhone { get; set; }
        public string RelativeName { get; set; }
        public string RelationShip { get;set; }
        public string Relative_Address { get;set; }

        public string BankAccountNo { get; set; }

        public string BankName { get;set; }
        public long RelationId { get; set;}

        public string BankId { get; set; }

        public string AccountTitle { get; set; }
        public string BranchName { get; set; }

        public List<EducationViewModel> EducationHistory { get; set; }
        public List<ExperienceViewModel> ExperienceHistory { get; set; }
        public Employee Employee { get; set; }
    }

    public class EducationViewModel
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class ExperienceViewModel
    {
        public string Company_name { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string TotalExperience { get; set; }

    }


    public class ViewBankViewModel
    {
        public List<Bank> Banks { get; set; }
        public List<Employee>  Employees { get; set; }
    }


}