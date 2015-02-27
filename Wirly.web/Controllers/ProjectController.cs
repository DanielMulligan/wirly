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

namespace Wirly.web.Controllers
{
    public class ProjectController : Controller
    {
        
        [Route("project/{id}")]
        public ActionResult Index(int id)
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
                return View(project);
            }
        }


        [HttpPost]
        [Route("project/{id}")]
        public ActionResult Index(int id, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                var f = new Wirly.web.Models.File
                {
                    Name = fileName,
                    Description = "",
                    Path = path
                };
                
                file.SaveAs(path);
                var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();
                var project = db.Projects.Find(id);
                project.Files.Add(f);
             
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}