using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.ViewModels;
using System.Diagnostics;

namespace HRMS.Controllers
{
    public class PrintController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Print
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrintEmployee(string id)
        {
            var employee = (from e in db.Employees
                            join ei in db.EmployeeInfos on e.Emp_id equals ei.Emp_Id
                            join c in db.Contacts on e.Emp_id equals c.Emp_Id
                            join l in db.Logins on e.Emp_id equals l.Emp_Id
                            where e.Emp_id == id
                            select new AddEmployeeViewModel
                            {
                                Emp_Id = e.Emp_id,
                                First_Name = e.First_Name,
                                Last_Name = e.Last_Name,
                                FatherHusbandName = e.Father_s_Husband_s_Name,
                                Gender = e.Gender,
                                DOB = e.DOB,
                                Marital_Status = e.Marital_Status,
                                Emp_Image = e.Emp_Image, // Store the image address
                                Religions = e.Religions,
                                Nationality = e.Nationality,
                                Blood_Group = e.Blood_Group,
                                Joining_date = ei.Joining_date,
                                JobTitle = ei.Job_Title,
                                email = c.Email,
                                Username = l.Username,
                                Password = l.Password,
                                phone = c.Mobile_No,
                                landline = c.Land_Line,
                                Probation = ei.Probation,
                                Probation_Confirmation_Date = ei.Probation_Confirmation_Date,
                                Location = ei.Location,
                                DepatmentId = ei.Department_Id,
                                Branch_id = ei.Branch_id,
                                Designation = ei.Designation,
                                Official_Email = ei.Official_Email,
                                Employment_Type = ei.Employment_Type,
                                Machine_Code = ei.Machine_Code
                            }).FirstOrDefault();

            if (employee == null)
            {
                return HttpNotFound();
            }

            // Since the image path is stored as "~/Content/upload/21359692WhatsApp Image 2024-07-04 at 10.09.03 AM (1).jpeg"
            // and assuming it's relative to the application's root, you can resolve it using Url.Content
            employee.Emp_Image_Full_Url = Url.Content(employee.Emp_Image);

            // Debugging output
            Debug.WriteLine($"Employee ID: {employee.Emp_Id}");
            Debug.WriteLine($"Employee Image Path: {employee.Emp_Image_Full_Url}");

            return new ViewAsPdf("EmployeeDetailPdf", employee)
            {
                FileName = $"Employee_{employee.Emp_Id}.pdf"
            };
        }
    }
}
