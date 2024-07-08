using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using System.Web.Security;
using HRMS.Functions;

namespace HRMS.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve user credentials from database
                var user = db.Logins.FirstOrDefault(u => u.Username == model.Username);

                if (user != null && HashPass.VerifyPassword(model.Password, user.Password))
                {
                    // Retrieve employee info based on EmployeeId from Login table
                    var employeeInfo = db.EmployeeInfos.FirstOrDefault(e => e.Emp_Id == user.Emp_Id);

                    if (employeeInfo != null)
                    {
                        // Ensure employeeInfo.Designation is retrieved correctly
                        string designation = employeeInfo.Designation != null ? employeeInfo.Designation.ToString() : "Unknown";

                        // Create authentication ticket with user designation
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                            1,                                    // Ticket version
                            user.Username,                        // Username
                            DateTime.Now,                         // Issue date
                            DateTime.Now.AddMinutes(60),          // Expiration date
                            false,                                // Persistent
                            designation,                          // User data (store user designation)
                            FormsAuthentication.FormsCookiePath   // Cookie path
                        );

                        // Encrypt the ticket
                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                        // Create the cookie
                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        Response.Cookies.Add(authCookie);

                        // Redirect user to dashboard or desired page after login
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee information not found.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form with errors
            return View(model);
        }
    }
}
