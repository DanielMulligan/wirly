using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wirly.web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Wirly.web.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using PagedList;

namespace Wirly.web.Controllers
{
    public class ProjectController : Controller
    {   
        [Route("project/{id}")]
        public ActionResult Index(int id, int? page)
        {
            var loggedOnUserId = User.Identity.GetUserId();
            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();

            //var project = db.Projects.SingleOrDefault(p => (p.Id == id && p.Users.Select(u => u.Id).Contains(loggedOnUserId)));
            var project = db.Projects.Find(id);

            if (!project.Users.Any(u => u.Id == loggedOnUserId) && !User.IsInRole("admin"))
            {
                return new RedirectResult("/");
            }
            else
            {
                var docTypes = db.DocumentTypes.ToList();
                ViewBag.Body = project.Body;
                ViewBag.Name = project.Name;
                ViewBag.DocTypes = docTypes;

                int pageSize = 3;
                int pageNumber = (page ?? 1);
                var pagedList = project.Documents.ToPagedList(pageNumber, pageSize);

                return View(pagedList);
            }
        }

        [HttpPost]
        [Route("project/{id}/AddHtml")]
        [ValidateInput(false)]
        public ActionResult AddHtml(int id, string html, string docName, string description)
        {
            var f = new Wirly.web.Models.Document
            {
                Name = docName,
                Description = description, 
                Html = html,
                DocumentType = "html"
            };

            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();
            var project = db.Projects.Find(id);
            project.Documents.Add(f);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("project/{id}/AddPdf")]
        public ActionResult AddPdf(int id, HttpPostedFileBase file, string docName, string description)
        {
            if (file.ContentLength > 0)
            {
                var f = new Wirly.web.Models.Document
                {
                    Name = docName,
                    Description = description,
                    DocumentType = "pdf"
                };

                var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();
                var project = db.Projects.Find(id);
                project.Documents.Add(f);

                db.SaveChanges();
                var ext = System.IO.Path.GetExtension(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Uploads"), f.Id.ToString() + ext);
                file.SaveAs(path);
                f.Path = string.Format("/uploads/{0}", f.Id.ToString() + ext);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("project/deletedocument")]
        public void DeleteDocument(int id)
        {
            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();

            var file = db.Documents.Find(id);
            if(file != null)
            {
                var docPath = Server.MapPath(file.Path);
                System.IO.File.Delete(docPath);
                db.Documents.Remove(file);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("project/GetItemHtml/{id}")]
        public string GetItemHtml(int id)
        {
            var db = Request.GetOwinContext().Get<WirlyDbContext>();
            var doc = db.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
            {
                return "";
            }
            else
            {
                return doc.Html;
            }
        }
    }
}