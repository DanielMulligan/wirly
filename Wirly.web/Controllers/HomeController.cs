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
    [Route("{action=Index}")]
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var loggedOnUser = await userManager.FindByIdAsync(User.Identity.GetUserId());
            
            return View(loggedOnUser.Projects);
        }
    }
}