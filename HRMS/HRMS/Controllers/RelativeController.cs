using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class RelativeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Education/ViewEducation
        public ActionResult ViewRelative(string searchBy, string searchValue)
        {
            var Relative = db.Relatives.Include("Employee").ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        Relative = Relative.Where(e => e.Emp_Id.Contains(searchValue)).ToList();
                        break;
                    case "ByRelativeName":
                        Relative = Relative.Where(e => e.Name.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new AddRelativeViewModel
            {
                Employees = db.Employees.ToList(),
                AddRelative = new Relative(),
                Relative = Relative
            };

            return View(model);
        }

        // GET: Education/AddEducation
        public ActionResult AddRelative()
        {
            var model = new AddRelativeViewModel
            {
                AddRelative = new Relative(),
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // POST: Education/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRelative(AddRelativeViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Relatives.Add(model.AddRelative);
                db.SaveChanges();
                return RedirectToAction("ViewRelative");
            }

            model.Relative = db.Relatives.ToList();
            return View(model);
        }

        // GET: Education/EditEducation
        public ActionResult EditRelative(long id)
        {
            var Relative = db.Relatives.Find(id);
            if (Relative == null)
            {
                return HttpNotFound();
            }

            return View(Relative);
        }

        // POST: Education/EditEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRelative(Relative model)
        {
            if (ModelState.IsValid)
            {
                var RelativeToUpdate = db.Relatives.Find(model.Relative_id);
                if (RelativeToUpdate != null)
                {
                    RelativeToUpdate.Name = model.Name;
                    RelativeToUpdate.Relationship = model.Relationship;
                    RelativeToUpdate.Address = model.Address;
                    RelativeToUpdate.Mobile_No = model.Mobile_No;
                    RelativeToUpdate.Contact_Type = model.Contact_Type;

                    db.SaveChanges();
                    return RedirectToAction("ViewRelative");
                }
            }

            return View(model);
        }

        // POST: Education/DeleteEducation
        [HttpPost]
        public ActionResult DeleteRelative(long id)
        {
            var Relative = db.Relatives.Find(id);
            if (Relative != null)
            {
                db.Relatives.Remove(Relative);
                db.SaveChanges();
            }

            return RedirectToAction("ViewRelative");
        }
    }
}