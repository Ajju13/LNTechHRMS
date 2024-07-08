using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class EducationController : Controller
    {
        // GET: Education
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Education/ViewEducation
        public ActionResult ViewEducation(string searchBy, string searchValue)
        {
            var educations = db.Educations.Include("Employee").ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        educations = educations.Where(e => e.Emp_id.Contains(searchValue)).ToList();
                        break;
                    case "ByUniversityCollegeName":
                        educations = educations.Where(e => e.University_College_Name.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new AddEducationViewModel
            {
                Employees = db.Employees.ToList(),
                AddEducation = new Education(),
                Educations = educations
            };

            return View(model);
        }

        // GET: Education/AddEducation
        public ActionResult AddEducation()
        {
            var model = new AddEducationViewModel
            {
                AddEducation = new Education(),
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // POST: Education/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEducation(AddEducationViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Educations.Add(model.AddEducation);
                db.SaveChanges();
                return RedirectToAction("ViewEducation");
            }

            model.Employees = db.Employees.ToList();
            return View(model);
        }

        // GET: Education/EditEducation
        public ActionResult EditEducation(string empId)
        {
            var education = db.Educations.FirstOrDefault(e => e.Emp_id == empId);
            if (education == null)
            {
                return HttpNotFound();
            }

            return View(education);
        }

        // POST: Education/EditEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEducation(Education model)
        {
            if (ModelState.IsValid)
            {
                var educationToUpdate = db.Educations.Find(model.Education_id);
                if (educationToUpdate != null)
                {
                    educationToUpdate.University_College_Name = model.University_College_Name;
                    educationToUpdate.Degree = model.Degree;
                    educationToUpdate.From_Date = model.From_Date;
                    educationToUpdate.To_Date = model.To_Date;

                    db.SaveChanges();
                    return RedirectToAction("ViewEducation");
                }
            }

            return View(model);
        }

        // POST: Education/DeleteEducation
        [HttpPost]
        public ActionResult DeleteEducation(long id)
        {
            var education = db.Educations.Find(id);
            if (education != null)
            {
                db.Educations.Remove(education);
                db.SaveChanges();
            }

            return RedirectToAction("ViewEducation");
        }
    }
}