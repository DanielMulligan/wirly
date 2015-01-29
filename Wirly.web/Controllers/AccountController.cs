using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wirly.web.Infrastructure;
using Wirly.web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Wirly.web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
   
        [AllowAnonymous]     
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturlUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel details, string returlUrl)
        {
            if(ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.Name, details.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent= false}, ident);

                    return Redirect(user.StartPage ?? "/home/index/");
                }
            }
            ViewBag.ReturlUrl = returlUrl;
            return View(details);
        }

        private AppUserManager UserManager
        {
            get {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}