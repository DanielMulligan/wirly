using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wirly.web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Wirly.web.Models;
using Microsoft.AspNet.Identity;

namespace Wirly.web.Controllers 
{
    [Route("admin/project/{action}")]
    public class ProjectAdminController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string Name, string Description)
        {
            WirlyDbContext db = HttpContext.GetOwinContext().Get<WirlyDbContext>();

            var projectWithSameNameExists = db.Projects.Any(p => p.Name == Name);
            if (projectWithSameNameExists)
            {
                ModelState.AddModelError("", "There is already a project with that name");
            }
            else
            {
                db.Projects.Add(new Project()
                {
                    Name = Name,
                    Description = Description
                });
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Route("admin/project")]
        public ActionResult Index()
        {
            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();

            return View(db.Projects);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();
            var project = await db.Projects.FindAsync(id);

            if (project != null)
            {
                var usersInProject = project.Users.Select(u => new UserAssignModel(){
                                               Id = u.Id,
                                               FirstName = u.FirstName,
                                               LastName = u.LastName,
                                               Email = u.Email
                                            }) ?? new List<UserAssignModel>();

                var userIdsInProject = usersInProject.Select(up => up.Id).ToList<string>();

                var userNotInProject = UserManager.Users.Where(u => !userIdsInProject.Contains(u.Id)).Select(u => new UserAssignModel()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();

                return View( new ProjectEditModel(){
                    Project = project,
                    Members = usersInProject,
                    NonMembers = userNotInProject
                });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(int id, string Description, string Body, string[] usersInProject)
        {
            var db = HttpContext.GetOwinContext().Get<WirlyDbContext>();

            var project = await db.Projects.FindAsync(id);
            if (project != null)
            {
                project.Description = Description;
                project.Body = Body;
                project.Users.Clear();
                //remove all users assigned to this project
                var usersToAdd = new List<AppUser>();
                
                //then only add back in the selected users
                foreach(string userId in usersInProject){
                    var userToAdd = db.Users.Find(userId);
                    if(userToAdd != null){
                        project.Users.Add(userToAdd);
                    }     
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}
