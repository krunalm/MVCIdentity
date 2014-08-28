using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityManager;

namespace MVCIdentity.Models
{
    public class AspNetIdentityIdentityManagerFactory
    {
        public IIdentityManagerService Create()
        {
            #region MyIdentity Stuff
            MyIdentityDbContext db = new MyIdentityDbContext();
            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db);
            UserManager<MyIdentityUser> userManager = new UserManager<MyIdentityUser>(userStore);

            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db);
            RoleManager<MyIdentityRole> roleManager = new RoleManager<MyIdentityRole>(roleStore);
            #endregion

            var svc = new Thinktecture.IdentityManager.AspNetIdentity.AspNetIdentityManagerService<MyIdentityUser, string, MyIdentityRole, string>(userManager, roleManager);

            return new DisposableIdentityManagerService(svc, db);
        }
    }
}