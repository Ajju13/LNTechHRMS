using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class ExperienceController : Controller
    {
        // GET: Experience
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Education/ViewEducation
        public ActionResult ViewExperience(string searchBy, string searchValue)
        {
            var experience = db.Experiences.Include("Employee").ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        experience = experience.Where(e => e.Emp_Id.Contains(searchValue)).ToList();
                        break;
                    case "ByCompanyName":
                        experience = experience.Where(e => e.Company_name.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new AddExperienceViewModel
            {
                Employees = db.Employees.ToList(),
                AddExperience = new Experience(),
                Experience = experience
            };

            return View(model);
        }

        // GET: Education/AddEducation
        public ActionResult AddExperience()
        {
            var model = new AddExperienceViewModel
            {
                AddExperience = new Experience(),
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // POST: Education/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExperience(AddExperienceViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Experiences.Add(model.AddExperience);
                db.SaveChanges();
                return RedirectToAction("ViewExperience");
            }

            model.Experience = db.Experiences.ToList();
            return View(model);
        }

        // GET: Education/EditEducation
        public ActionResult EditExperience(string empId)
        {
            var experience = db.Experiences.FirstOrDefault(e => e.Emp_Id == empId);
            if (experience == null)
            {
                return HttpNotFound();
            }

            return View(experience);
        }

        // POST: Education/EditEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExperience(Experience model)
        {
            if (ModelState.IsValid)
            {
                var experinceToUpdate = db.Experiences.Find(model.Exp_id);
                if (experinceToUpdate != null)
                {
                    experinceToUpdate.Company_name = model.Company_name;
                    experinceToUpdate.Position = model.Position;
                    experinceToUpdate.From_Date = model.From_Date;
                    experinceToUpdate.To_Date = model.To_Date;

                    db.SaveChanges();
                    return RedirectToAction("ViewExperience");
                }
            }

            return View(model);
        }

        // POST: Education/DeleteEducation
        [HttpPost]
        public ActionResult DeleteExperience(long id)
        {
            var experience = db.Experiences.Find(id);
            if (experience != null)
            {
                db.Experiences.Remove(experience);
                db.SaveChanges();
            }

            return RedirectToAction("ViewExperience");
        }
    }
}