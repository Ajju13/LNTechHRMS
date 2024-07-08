using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.ViewModels;
using System.Diagnostics.Contracts;
using System.Data.Entity.Validation;
using System.IO;
using HRMS.Functions;
using System.Web.Security;
using System.Net.Sockets;
using static HRMS.ViewModels.PersonalProfileViewModel;
using System.Data.Entity;

namespace HRMS.Controllers
{
    public class DashboardController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult My()
        {
            // Get the logged-in user's username
            var authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Decrypt the ticket
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Get the username from the ticket
            string username = ticket.Name;

            // Retrieve the employee's information
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
                                where l.Username == username
                                select new
                                {
                                    Employee = e,
                                    EmployeeInfo = ei,
                                    Contact = c,
                                    Login = l,
                                    CNIC = cnic,
                                    Relative = relative,
                                    Bank = bank
                                }).FirstOrDefault();

            if (employeeData == null)
            {
                return HttpNotFound();
            }

            // Construct the full URL for the image
            employeeData.Employee.Emp_Image = Url.Content(employeeData.Employee.Emp_Image);

            // Retrieve education history
            var educationHistory = (from edu in db.Educations
                                    where edu.Emp_id == employeeData.Employee.Emp_id
                                    select new EducationViewModel
                                    {
                                        InstitutionName = edu.University_College_Name,
                                        Degree = edu.Degree,
                                        FromDate = edu.From_Date,
                                        ToDate = edu.To_Date
                                    }).ToList();

            // Retrieve experience history
            var experienceHistory = (from exp in db.Experiences
                                     where exp.Emp_Id == employeeData.Employee.Emp_id
                                     select new ExperienceViewModel
                                     {
                                         Company_name = exp.Company_name,
                                         Position = exp.Position,
                                         FromDate = exp.From_Date,
                                         ToDate = exp.To_Date
                                     }).ToList();

            // Calculate total experience for each experience entry
            foreach (var exp in experienceHistory)
            {
                exp.TotalExperience = CalculateTotalExperience(exp.FromDate, exp.ToDate);
            }

            // Create the view model
            var viewModel = new PersonalProfileViewModel
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
                Landline = employeeData?.Contact.Land_Line,
                Location = employeeData?.EmployeeInfo.Location,
                Machine_Code = employeeData?.EmployeeInfo.Machine_Code,
                CNICNo = employeeData?.CNIC != null ? employeeData.CNIC.CNIC_Number : "N/A",
                RelativeName = employeeData?.Relative != null ? employeeData.Relative.Name : "N/A",
                RelationShip = employeeData?.Relative != null ? employeeData.Relative.Relationship : "N/A",
                Relative_Address = employeeData?.Relative != null ? employeeData.Relative.Address : "N/A",
                RelatvePhone = employeeData?.Relative != null ? employeeData.Relative.Mobile_No : "N/A",
                BankName = employeeData?.Bank != null ? employeeData.Bank.Bank_Name : "N/A",
                BankAccountNo = employeeData?.Bank != null ? employeeData.Bank.Account_No : "N/A",
                AccountTitle = employeeData?.Bank != null ? employeeData.Bank.Account_Title : "N/A",
                BranchName = employeeData?.Bank != null ? employeeData.Bank.Branch_Name : "N/A",
                EducationHistory = educationHistory, // Assuming educationHistory is populated elsewhere
                ExperienceHistory = experienceHistory
            };

            return View(viewModel);
        }

        private string CalculateTotalExperience(DateTime fromDate, DateTime toDate)
        {
            var diff = toDate.Subtract(fromDate);
            int years = (int)(diff.Days / 365.25);
            int months = (int)((diff.Days % 365.25) / 30.4375); // Average days per month
            return $"{years} year{(years != 1 ? "s" : "")} and {months} month{(months != 1 ? "s" : "")}";
        }
        public ActionResult ViewEmployee(string searchBy, string searchValue)
        {
            // Fetch all employees from the database
            var employees = db.Employees.ToList();

            // Fetch related data separately
            var logins = db.Logins.ToList();
            var contacts = db.Contacts.ToList();
            var employeeInfos = db.EmployeeInfos.ToList();

            // Perform a left join-like operation to combine data into EmployeeViewModel
            var employeeViewModels = employees.Select(e => new EmployeeViewModel
            {
                Emp_Image = e.Emp_Image,
                Emp_id = e.Emp_id,
                Name = e.First_Name + " " + e.Last_Name,
                Email = logins.FirstOrDefault(l => l.Emp_Id == e.Emp_id)?.Email ?? string.Empty,
                Phone = contacts.FirstOrDefault(c => c.Emp_Id == e.Emp_id)?.Mobile_No ?? string.Empty,
                JoinDate = employeeInfos.FirstOrDefault(f => f.Emp_Id == e.Emp_id)?.Joining_date ?? DateTime.MinValue,
                JobTitle = employeeInfos.FirstOrDefault(f => f.Emp_Id == e.Emp_id)?.Job_Title ?? string.Empty
            }).ToList();

            // Apply search filters if searchBy and searchValue are provided
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchValue))
            {
                if (searchBy == "ByEmployeeID")
                {
                    employeeViewModels = employeeViewModels.Where(e => e.Emp_id.Contains(searchValue)).ToList();
                }
                else if (searchBy == "ByEmployeeName")
                {
                    employeeViewModels = employeeViewModels.Where(e => e.Name.Contains(searchValue)).ToList();
                }
            }

            var model = new ViewEmployeePageModel
            {
                Employees = employeeViewModels,
                AddEmployee = new AddEmployeeViewModel(),
                Departments = db.Department.ToList(),
                Branches = db.Branch.ToList()
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult DeleteEmployee(string id)
        {
            var Emp = db.Employees.Find(id);
            if (Emp != null)
            {
                var Einfo = db.EmployeeInfos.Where(e => e.Emp_Id == Emp.Emp_id);
                var Con = db.Contacts.Where(e => e.Emp_Id == Emp.Emp_id);
                var log = db.Logins.Where(e => e.Emp_Id == Emp.Emp_id);
                db.EmployeeInfos.RemoveRange(Einfo);
                db.Contacts.RemoveRange(Con);
                db.Logins.RemoveRange(log);
                db.Employees.Remove(Emp);
                db.SaveChanges();
            }
            return RedirectToAction("ViewEmployee");
        }


        public ActionResult SearchEmployees(string searchBy, string searchValue)
        {
            // Fetch all employees from the database
            var employees = db.Employees.ToList();

            // Apply search filters based on provided parameters
            var filteredEmployees = employees
                .Where(e =>
                    (searchBy == "ByEmployeeID" && (string.IsNullOrEmpty(searchValue) || e.Emp_id.Contains(searchValue))) ||
                    (searchBy == "ByEmployeeName" && (string.IsNullOrEmpty(searchValue) || (e.First_Name + " " + e.Last_Name).Contains(searchValue)))
                )
                .ToList();

            // Map the filtered employees to EmployeeViewModels
            var employeeViewModels = filteredEmployees.Select(e => new EmployeeViewModel
            {
                Emp_id = e.Emp_id,
                Name = e.First_Name + " " + e.Last_Name,
                // Include other properties you need in the view model
            }).ToList();

            // Prepare the model for the view
            var model = new ViewEmployeePageModel
            {
                Employees = employeeViewModels,
                AddEmployee = new AddEmployeeViewModel(), // Assuming you need this in search results
                Departments = db.Department.ToList(),     // Assuming departments are needed in search results
                Branches = db.Branch.ToList()             // Assuming branches are needed in search results
            };

            // Return the view with the filtered and mapped data
            return View("ViewEmployee", model);
        }

        [HttpPost]
        public ActionResult AddEmployee(AddEmployeeViewModel model, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Find the highest existing employee ID
                    var highestEmployee = db.Employees.OrderByDescending(e => e.Emp_id).FirstOrDefault();
                    int nextIdNumber = 1;

                    if (highestEmployee != null)
                    {
                        // Extract the number part from the existing highest ID
                        if (int.TryParse(highestEmployee.Emp_id.Substring(3), out int currentNumber))
                        {
                            nextIdNumber = currentNumber + 1;
                        }
                    }

                    // Generate the new Employee ID in the format LN-XX
                    string newEmpId = $"LN-{nextIdNumber.ToString("00")}";

                    var highestLogin = db.Logins.OrderByDescending(l => l.Login_ID).FirstOrDefault();
                    int nextLoginId = 1;

                    string path = UploadImage(imgfile);

                    if (highestLogin != null)
                    {
                        nextLoginId = highestLogin.Login_ID + 1;
                    }

                    // Create new Employee entity
                    var employee = new Employee
                    {
                        Emp_id = newEmpId,
                        First_Name = model.First_Name,
                        Last_Name = model.Last_Name,
                        Emp_Image = path,
                        Father_s_Husband_s_Name = model.FatherHusbandName,
                        Gender = model.Gender,
                        DOB = model.DOB, // Assuming model.DOB is DateTime in your ViewModel
                        Marital_Status = model.Marital_Status,
                        Religions = model.Religions,
                        Nationality = model.Nationality,
                        Blood_Group = model.Blood_Group
                    };

                    db.Employees.Add(employee);

                    // Hash the password before creating the Login entity
                    string hashedPassword = HashPass.HashPassword(model.Password);

                    // Create new Login entity
                    var login = new Login
                    {
                        Login_ID = nextLoginId,
                        Emp_Id = newEmpId,
                        Email = model.email,
                        Username = model.Username,
                        Password = hashedPassword, // Use the hashed password
                        VerificationToken = "Null",
                        EmailConfirmed = false,
                        ResetPasswordCode = "Null",
                        ResetPasswordExpiry = "Null"
                    };

                    db.Logins.Add(login);

                    // Create new Contact entity
                    var contact = new Contact
                    {
                        Email = model.email,
                        Land_Line = model.landline,
                        Emp_Id = newEmpId,
                        Mobile_No = model.phone
                    };

                    db.Contacts.Add(contact);

                    // Create new EmployeeInfo entity
                    var employeeInfo = new Employee_info
                    {
                        Emp_Id = newEmpId,
                        Joining_date = model.Joining_date, // Assuming model.Joining_date is DateTime in your ViewModel
                        Job_Title = model.JobTitle,
                        Probation = model.Probation,
                        Probation_Confirmation_Date = model.Probation_Confirmation_Date,
                        Location = model.Location,
                        Department_Id = model.DepatmentId,
                        Branch_id = model.Branch_id,
                        Designation = model.Designation,
                        Official_Email = model.Official_Email,
                        Employment_Type = model.Employment_Type,
                        Machine_Code = model.Machine_Code
                    };

                    db.EmployeeInfos.Add(employeeInfo);

                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to the ViewEmployee action after successful addition
                    return RedirectToAction("ViewEmployee");
                }
                catch (Exception ex)
                {
                    // Handle exceptions (log them, show error messages, etc.)
                    ModelState.AddModelError("", "An error occurred while adding the employee. Please try again.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Emp_id == id);
            var contact = db.Contacts.FirstOrDefault(c => c.Emp_Id == id);
            var employeeInfo = db.EmployeeInfos.FirstOrDefault(ei => ei.Emp_Id == id);
            var login = db.Logins.FirstOrDefault(l => l.Emp_Id == id);

            if (employee == null || contact == null || employeeInfo == null || login == null)
            {
                return HttpNotFound();
            }

            var model = new EditEmployeeViewModel
            {
                Emp_Id = employee.Emp_id,
                First_Name = employee.First_Name,
                Last_Name = employee.Last_Name,
                FatherHusbandName = employee.Father_s_Husband_s_Name,
                Gender = employee.Gender,
                DOB = employee.DOB,
                Marital_Status = employee.Marital_Status,
                Religions = employee.Religions,
                Nationality = employee.Nationality,
                Blood_Group = employee.Blood_Group,
                phone = contact.Mobile_No,
                landline = contact.Land_Line,
                email = login.Email,
                Joining_date = employeeInfo.Joining_date,
                JobTitle = employeeInfo.Job_Title,
                Probation = employeeInfo.Probation,
                Probation_Confirmation_Date = employeeInfo.Probation_Confirmation_Date,
                Location = employeeInfo.Location,
                DepatmentId = employeeInfo.Department_Id,
                Branch_id = employeeInfo.Branch_id,
                Designation = employeeInfo.Designation,
                Official_Email = employeeInfo.Official_Email,
                Employment_Type = employeeInfo.Employment_Type,
                Machine_Code = employeeInfo.Machine_Code,
                Username = login.Username
            };

            ViewBag.Departments = db.Department.ToList();
            ViewBag.Branches = db.Branch.ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult EditEmployee(EditEmployeeViewModel model, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = db.Employees.FirstOrDefault(e => e.Emp_id == model.Emp_Id);
                    var contact = db.Contacts.FirstOrDefault(c => c.Emp_Id == model.Emp_Id);
                    var employeeInfo = db.EmployeeInfos.FirstOrDefault(ei => ei.Emp_Id == model.Emp_Id);
                    var login = db.Logins.FirstOrDefault(l => l.Emp_Id == model.Emp_Id);

                    if (employee == null || contact == null || employeeInfo == null || login == null)
                    {
                        ModelState.AddModelError("", "Employee details not found.");
                        return RedirectToAction("ViewEmployee");
                    }

                    string path = UploadImage(imgfile);
                    // Update Employee entity
                    employee.First_Name = model.First_Name;
                    employee.Last_Name = model.Last_Name;
                    employee.Father_s_Husband_s_Name = model.FatherHusbandName;
                    employee.Gender = model.Gender;
                    employee.DOB = model.DOB;
                    employee.Marital_Status = model.Marital_Status;
                    employee.Religions = model.Religions;
                    employee.Nationality = model.Nationality;
                    employee.Blood_Group = model.Blood_Group;

                    // Update Contact entity
                    contact.Mobile_No = model.phone;
                    contact.Land_Line = model.landline;
                    login.Email = model.email;

                    // Update EmployeeInfo entity
                    employeeInfo.Joining_date = model.Joining_date;
                    employeeInfo.Job_Title = model.JobTitle;
                    employeeInfo.Probation = model.Probation;
                    employeeInfo.Probation_Confirmation_Date = model.Probation_Confirmation_Date;
                    employeeInfo.Location = model.Location;
                    employeeInfo.Department_Id = model.DepatmentId;
                    employeeInfo.Branch_id = model.Branch_id;
                    employeeInfo.Designation = model.Designation;
                    employeeInfo.Official_Email = model.Official_Email;
                    employeeInfo.Employment_Type = model.Employment_Type;
                    employeeInfo.Machine_Code = model.Machine_Code;

                    // Update Login entity
                    login.Username = model.Username;

                    if (path != null && path.Length > 0)
                    {
                        employee.Emp_Image = path;
                    }

                    db.SaveChanges();

                    return RedirectToAction("ViewEmployee");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the employee. Please try again.");
                }
            }

            ViewBag.Departments = db.Department.ToList();
            ViewBag.Branches = db.Branch.ToList();

            return View(model);
        }


        public ActionResult ViewBank(string searchBy, string searchValue)
        {
            var banks = db.Banks.Include(b => b.Employee).ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        banks = banks.Where(b => b.Emp_Id.Contains(searchValue)).ToList();
                        break;
                    case "ByBankName":
                        banks = banks.Where(b => b.Bank_Name.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new BankViewModel
            {
                Banks = banks,
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // GET: AddBank
        public ActionResult AddBank()
        {
            var model = new BankViewModel
            {
                AddBank = new Bank(),
                Employees = db.Employees.ToList()
            };
            return View(model);
        }

        // POST: AddBank
        [HttpPost]
        public ActionResult AddBank(BankViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AddBank.Bank_id = Guid.NewGuid().ToString();
                db.Banks.Add(model.AddBank);
                db.SaveChanges();
                return RedirectToAction("ViewBank");
            }

            model.Employees = db.Employees.ToList();
            return View(model);
        }

        // POST: DeleteBank
        [HttpPost]
        public ActionResult DeleteBank(string id)
        {
            var bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }

            db.Banks.Remove(bank);
            db.SaveChanges();
            return RedirectToAction("ViewBank");
        }

        // GET: Dashboard/EditBank
        public ActionResult EditBank(string id)
        {
            var bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }

            return View(bank);
        }

        // POST: Dashboard/EditBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBank(Bank bank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewBank");
            }

            return View(bank);
        }

        public string UploadImage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                        // Log the exception or handle it appropriately
                    }
                }
                else
                {

                    Response.Write("<script>alert('Only jpg, jpeg, or png formats are acceptable....');</script>");
                }
            }
            else
            {

                Response.Write("<script>alert('Please select a file');</script>");
            }

            return path;
        }


    }

}


