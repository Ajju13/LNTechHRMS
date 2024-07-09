using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.ViewModels;
using System.Threading.Tasks;

namespace HRMS.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult EmployeeProfile(string searchValue)
        {
            PersonalProfileViewModel viewModel = new PersonalProfileViewModel();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var employeeData = (from e in db.Employees
                                    join ei in db.EmployeeInfos on e.Emp_id equals ei.Emp_Id
                                    join c in db.Contacts on e.Emp_id equals c.Emp_Id
                                    join l in db.Logins on e.Emp_id equals l.Emp_Id
                                    join d in db.CNIC on e.Emp_id equals d.Emp_Id into cnicDocs
                                    from cnic in cnicDocs.DefaultIfEmpty()
                                    join r in db.Relatives on e.Emp_id equals r.Emp_Id into relatives
                                    from relative in relatives.DefaultIfEmpty()
                                    join b in db.Banks on e.Emp_id equals b.Emp_Id into banks
                                    from bank in banks.DefaultIfEmpty()
                                    join s in db.Salaries on e.Emp_id equals s.Emp_id into Salary
                                    from sal in Salary.DefaultIfEmpty()
                                    where e.Emp_id == searchValue
                                    select new
                                    {
                                        Employee = e,
                                        EmployeeInfo = ei,
                                        Contact = c,
                                        Login = l,
                                        CNIC = cnic,
                                        Relative = relative,
                                        Bank = bank,
                                        Salary = sal
                                    }).FirstOrDefault();

                 if (employeeData != null)
                {
                    employeeData.Employee.Emp_Image = Url.Content(employeeData.Employee.Emp_Image);

                    var educationHistory = (from edu in db.Educations
                                            where edu.Emp_id == employeeData.Employee.Emp_id
                                            select new EducationViewModel
                                            {
                                                InstitutionName = edu.University_College_Name,
                                                Degree = edu.Degree,
                                                FromDate = edu.From_Date,
                                                ToDate = edu.To_Date
                                            }).ToList();

                    var experienceHistory = (from exp in db.Experiences
                                             where exp.Emp_Id == employeeData.Employee.Emp_id
                                             select new ExperienceViewModel
                                             {
                                                 Company_name = exp.Company_name,
                                                 Position = exp.Position,
                                                 FromDate = exp.From_Date,
                                                 ToDate = exp.To_Date
                                             }).ToList();

                    foreach (var exp in experienceHistory)
                    {
                        exp.TotalExperience = CalculateTotalExperience(exp.FromDate, exp.ToDate);
                    }

                    viewModel = new PersonalProfileViewModel
                    {
                        Emp_Id = employeeData?.Employee.Emp_id,
                        First_Name = employeeData?.Employee.First_Name,
                        Last_Name = employeeData?.Employee.Last_Name,
                        FatherHusbandName = employeeData?.Employee.Father_s_Husband_s_Name,
                        Gender = employeeData?.Employee.Gender,
                        DOB = employeeData.Employee.DOB,
                        Marital_Status = employeeData?.Employee.Marital_Status,
                        Emp_Image = employeeData?.Employee.Emp_Image,
                        Religions = employeeData?.Employee.Religions,
                        Nationality = employeeData?.Employee.Nationality,
                        Blood_Group = employeeData?.Employee.Blood_Group,
                        Joining_date = employeeData.EmployeeInfo.Joining_date,
                        JobTitle = employeeData?.EmployeeInfo.Job_Title,
                        DepartmentId = employeeData.EmployeeInfo.Department_Id,
                        BranchId = employeeData.EmployeeInfo.Branch_id,
                        Designation = employeeData?.EmployeeInfo.Designation,
                        Employment_Type = employeeData?.EmployeeInfo.Employment_Type,
                        Email = employeeData?.Login.Email,
                        Official_Email = employeeData?.EmployeeInfo.Official_Email,
                        Phone = employeeData?.Contact.Mobile_No,
                        Landline = employeeData?.Contact != null ? employeeData.Contact.Land_Line : "N/A",
                        Location = employeeData?.EmployeeInfo.Location,
                        Machine_Code = employeeData?.EmployeeInfo.Machine_Code,
                        CNICNo = employeeData?.CNIC != null ? employeeData.CNIC.CNIC_Number : "N/A",
                        RelationId = employeeData?.Relative?.Relative_id ?? -1,
                        RelativeName = employeeData?.Relative != null ? employeeData.Relative.Name : "N/A",
                        RelationShip = employeeData?.Relative != null ? employeeData.Relative.Relationship : "N/A",
                        Relative_Address = employeeData?.Relative != null ? employeeData.Relative.Address : "N/A",
                        RelatvePhone = employeeData?.Relative != null ? employeeData.Relative.Mobile_No : "N/A",
                        BankId = employeeData?.Bank != null ? employeeData.Bank.Bank_id : "N/A",
                        BankName = employeeData?.Bank != null ? employeeData.Bank.Bank_Name : "N/A",
                        BankAccountNo = employeeData?.Bank != null ? employeeData.Bank.Account_No : "N/A",
                        AccountTitle = employeeData?.Bank != null ? employeeData.Bank.Account_Title : "N/A",
                        BranchName = employeeData?.Bank != null ? employeeData.Bank.Branch_Name : "N/A",
                        After_Probation_Gross_Salary = employeeData?.Salary != null ? employeeData.Salary.After_Probation_Gross_Salary : 0,
                        Probation_Salary = employeeData?.Salary != null ? employeeData.Salary.Probation_Salary : 0,
                        NTN_Number = employeeData?.Salary != null ? employeeData.Salary.NTN_Number : "N/A",
                        Frequency = employeeData?.Salary != null ? employeeData.Salary.Frequency : "N/A",
                        Payment_Method = employeeData?.Salary != null ? employeeData.Salary.Payment_Method : "N/A",
                        Sal_id = employeeData?.Salary != null ? employeeData.Salary.Sal_id : 0,
                        EducationHistory = educationHistory, // Assuming educationHistory is populated elsewhere
                        ExperienceHistory = experienceHistory
                    };
                }
                else
                {
                    ViewBag.ErrorMessage = "Employee not found.";
                }
            }

            return View(viewModel);
        }

        private string CalculateTotalExperience(DateTime fromDate, DateTime toDate)
        {
            var diff = toDate.Subtract(fromDate);
            int years = (int)(diff.Days / 365.25);
            int months = (int)((diff.Days % 365.25) / 30.4375); // Average days per month
            return $"{years} year{(years != 1 ? "s" : "")} and {months} month{(months != 1 ? "s" : "")}";
        }

    }
}
