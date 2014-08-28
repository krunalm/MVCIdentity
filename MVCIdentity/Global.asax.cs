using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCIdentity
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db);
            RoleManager<MyIdentityRole> roleManager = new RoleManager<MyIdentityRole>(roleStore);

            if (!roleManager.RoleExists("Administrator"))
            {
                MyIdentityRole role = new MyIdentityRole("Administrator", "Administrators can add, edit and delete all items.");
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Operator"))
            {
                MyIdentityRole role = new MyIdentityRole("Operator", "Operator can only add or edit items.");
                roleManager.Create(role);
            }
        }
    }
}
