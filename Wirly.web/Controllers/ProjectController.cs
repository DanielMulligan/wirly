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
            var project = db.Projects.SingleOrDefault(p => (p.Id == id && p.Users.Select(u => u.Id).Contains(loggedOnUserId)));
            if (project == null)
            {
                return new RedirectResult("/");
            }
            else
            {
                ViewBag.Body = project.Body;
                ViewBag.Name = project.Name;

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                var pagedList = project.Files.ToPagedList(pageNumber, pageSize);

                return View(pagedList);
            }
        }

        [HttpPost]
        [Route("project/{id}")]
        public ActionResult Index(int id, HttpPostedFileBase file, string fileName, string description)
        {
            if (file.ContentLength > 0)
            {
                var f = new Wirly.web.Models.File
                {
                    Name = fileName,
                    Description = description
                };

                var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();
                var project = db.Projects.Find(id);
                project.Files.Add(f);

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

            var file = db.Files.Find(id);
            if(file != null)
            {
                var docPath = Server.MapPath(file.Path);
                System.IO.File.Delete(docPath);
                db.Files.Remove(file);
                db.SaveChanges();
            }
        }
    }
}