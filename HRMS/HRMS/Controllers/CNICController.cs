using HRMS.Models;
using HRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    public class CNICController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Education/ViewEducation
        public ActionResult ViewCNIC(string searchBy, string searchValue)
        {
            var CNIC = db.CNIC.Include("Employee").ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        CNIC = CNIC.Where(e => e.Emp_Id.Contains(searchValue)).ToList();
                        break;
                    case "ByCnicNo":
                        CNIC = CNIC.Where(e => e.CNIC_Number.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new AddCNICViewModel
            {
                Employees = db.Employees.ToList(),
                AddCNIC = new CNIC(),
                CNIC = CNIC
            };

            return View(model);
        }

        // GET: Education/AddEducation
        public ActionResult AddCNIC()
        {
            var model = new AddCNICViewModel
            {
                AddCNIC = new CNIC(),
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // POST: Education/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCNIC(AddCNICViewModel model, HttpPostedFileBase CNICFile)
        {
            if (ModelState.IsValid)
            {

                if (CNICFile != null && CNICFile.ContentLength > 0)
                {
                    Random r = new Random();
                    int random = r.Next();

                    // Get file extension
                    var extension = Path.GetExtension(CNICFile.FileName);

                    // Construct a unique filename
                    var fileName = random + "_" + Path.GetFileNameWithoutExtension(CNICFile.FileName) + extension;

                    // Save file to server
                    var path = Path.Combine(Server.MapPath("~/Content/upload/"), fileName);
                    CNICFile.SaveAs(path);

                    // Update model with file path
                    model.AddCNIC.Document_Attachment = "~/Content/upload/" + fileName;
                }

                db.CNIC.Add(model.AddCNIC);
                db.SaveChanges();
                return RedirectToAction("ViewCNIC");
            }

            model.Employees = db.Employees.ToList();
            model.CNIC = db.CNIC.ToList();
            return View(model);
        }

        // GET: Education/EditEducation
        public ActionResult EditCNIC(long id)
        {
            var CNIC = db.CNIC.Find(id);
            if (CNIC == null)
            {
                return HttpNotFound();
            }

            return View(CNIC);
        }

        // POST: Education/EditEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCNIC(CNIC model, HttpPostedFileBase CNICFile)
        {
            if (ModelState.IsValid)
            {
                var CNICToUpdate = db.CNIC.Find(model.Document_id);
                if (CNICToUpdate != null)
                {
                    if (CNICFile != null && CNICFile.ContentLength > 0)
                    {
                        Random r = new Random();
                        int random = r.Next();

                        // Get file extension
                        var extension = Path.GetExtension(CNICFile.FileName);

                        // Construct a unique filename
                        var fileName = random + "_" + Path.GetFileNameWithoutExtension(CNICFile.FileName) + extension;

                        // Save file to server
                        var path = Path.Combine(Server.MapPath("~/Content/upload/"), fileName);
                        CNICFile.SaveAs(path);

                        // Update model with file path
                        CNICToUpdate.Document_Attachment = "~/Content/upload/" + fileName;
                    }
                    CNICToUpdate.CNIC_Number = model.CNIC_Number;
                    CNICToUpdate.Document_Expiry = model.Document_Expiry;

                    db.SaveChanges();
                    return RedirectToAction("ViewCNIC");
                }
            }

            return View(model);
        }

        // POST: Education/DeleteEducation
        [HttpPost]
        public ActionResult DeleteCNIC(long id)
        {
            var CNIC = db.CNIC.Find(id);
            if (CNIC != null)
            {
                db.CNIC.Remove(CNIC);
                db.SaveChanges();
            }

            return RedirectToAction("ViewCNIC");
        }

        public ActionResult DownloadCNICDocument(int id)
        {
            var CNIC = db.CNIC.Find(id);
            if (CNIC == null || string.IsNullOrEmpty(CNIC.Document_Attachment))
            {
                return HttpNotFound();
            }

            var filePath = Server.MapPath(CNIC.Document_Attachment);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return HttpNotFound();
            }

            return File(filePath, MimeMapping.GetMimeMapping(filePath), CNIC.CNIC_Number + Path.GetExtension(filePath));
        }
    }
}