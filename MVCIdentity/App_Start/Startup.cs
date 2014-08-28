using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Thinktecture.IdentityManager;
using MVCIdentity.Models;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(MVCIdentity.App_Start.Startup))]

namespace MVCIdentity.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #region Common Auth Configuration
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            options.LoginPath = new PathString("/account/login");
            app.UseCookieAuthentication(options);
            #endregion

            app.Map("/admin", subapp =>
                {
                    subapp.UseIdentityManager(new IdentityManagerConfiguration()
                    {
                        IdentityManagerFactory = new AspNetIdentityIdentityManagerFactory().Create
                    });
                });

            //app.UseIdentityManager(new IdentityManagerConfiguration()
            //{
            //    IdentityManagerFactory = new AspNetIdentityIdentityManagerFactory().Create
            //});

            #region Thinktecture Stuff

            #endregion
        }
    }
}
