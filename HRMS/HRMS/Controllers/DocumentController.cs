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
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Education/ViewEducation
        public ActionResult ViewDocument(string searchBy, string searchValue)
        {
            var Document = db.EmpDocuments.Include("Employee").ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                switch (searchBy)
                {
                    case "ByEmployeeID":
                        Document = Document.Where(e => e.Emp_Id.Contains(searchValue)).ToList();
                        break;
                    case "ByDocumentName":
                        Document = Document.Where(e => e.Name.Contains(searchValue)).ToList();
                        break;
                }
            }

            var model = new AddDocumentViewModel
            {
                Employees = db.Employees.ToList(),
                AddDocument = new Emp_Documents(),
                Emp_Documents = Document,
                DocumentTypes = db.EmpDocumentTypes.ToList()
            };

            return View(model);
        }

        // GET: Education/AddEducation
        public ActionResult AddDocument()
        {
            var model = new AddDocumentViewModel
            {
                AddDocument = new Emp_Documents(),
                Employees = db.Employees.ToList()
            };

            return View(model);
        }

        // POST: Education/AddEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDocument(AddDocumentViewModel model, HttpPostedFileBase CNICFile)
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
                    model.AddDocument.File = "~/Content/upload/" + fileName;
                }

                db.EmpDocuments.Add(model.AddDocument);
                db.SaveChanges();
                return RedirectToAction("ViewDocument");
            }

            model.Employees = db.Employees.ToList();
            model.Emp_Documents = db.EmpDocuments.ToList();
            return View(model);
        }

        // GET: Education/EditEducation
        public ActionResult EditDocument(long id)
        {
            var Document = db.EmpDocuments.Find(id);
            if (Document == null)
            {
                return HttpNotFound();
            }

            return View(Document);
        }

        // POST: Education/EditEducation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDocument(Emp_Documents model, HttpPostedFileBase CNICFile)
        {
            if (ModelState.IsValid)
            {
                var DocumentToUpdate = db.EmpDocuments.Find(model.id);
                if (DocumentToUpdate != null)
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
                        DocumentToUpdate.File = "~/Content/upload/" + fileName;
                    }
                    DocumentToUpdate.Document_Type_id = model.Document_Type_id;
                    DocumentToUpdate.Name = model.Name;
                    DocumentToUpdate.Issue_Date = model.Issue_Date;

                    db.SaveChanges();
                    return RedirectToAction("ViewDocument");
                }
            }

            return View(model);
        }

        // POST: Education/DeleteEducation
        [HttpPost]
        public ActionResult DeleteDocument(long id)
        {
            var Document = db.EmpDocuments.Find(id);
            if (Document != null)
            {
                db.EmpDocuments.Remove(Document);
                db.SaveChanges();
            }

            return RedirectToAction("ViewDocument");
        }

        public ActionResult DownloadDocument(int id)
        {
            var Document = db.EmpDocuments.Find(id);
            if (Document == null || string.IsNullOrEmpty(Document.File))
            {
                return HttpNotFound();
            }

            var filePath = Server.MapPath(Document.File);
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
            {
                return HttpNotFound();
            }

            return File(filePath, MimeMapping.GetMimeMapping(filePath), Document.Name + Path.GetExtension(filePath));
        }
    }
}