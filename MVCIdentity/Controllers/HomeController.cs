using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIdentity.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db);
            UserManager<MyIdentityUser> userManager = new UserManager<MyIdentityUser>(userStore);

            MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);

            NorthWindEntities northwindDb = new NorthWindEntities();

            List<Customer> customers = null;

            if (userManager.IsInRole(user.Id, "Administrator"))
            {
                customers = northwindDb.Customers.ToList();
            }

            if (userManager.IsInRole(user.Id, "Operator"))
            {
                customers = northwindDb.Customers.Where(m => m.City == "USA").ToList();
            }

            ViewBag.FullName = user.FullName + " (" + user.UserName + ") !";
            return View(customers);
        }
    }
}