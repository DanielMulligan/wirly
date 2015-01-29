using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wirly.web.Infrastructure;
using Wirly.web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Wirly.web.Controllers
{
    public class RoleAdminController : Controller
    {
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
               }
        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIds = role.Users.Select(u => u.UserId).ToArray();
            
            IEnumerable<AppUser> members  = UserManager.Users.Where(u => memberIds.Any(mId => mId == u.Id));

            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);

            var model = new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", new string[] { "Role Not Found" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            if(role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role not found" });
            }
        }



    }

}