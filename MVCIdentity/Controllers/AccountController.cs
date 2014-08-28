using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MVCIdentity.Models;
using MVCIdentity.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Host.SystemWeb;
using System.Security.Claims;

namespace MVCIdentity.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        public AccountController()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db);
            userManager = new UserManager<MyIdentityUser>(userStore);

            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db);
            roleManager = new RoleManager<MyIdentityRole>(roleStore);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = new MyIdentityUser();

                user.UserName = model.Username;
                user.Email = model.Email;
                user.BirthDate = model.BirthDate;
                user.Bio = model.Bio;
                user.FullName = model.Fullname;

                IdentityResult result = userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Username", "Error while creating the user!");
                }
            }

            return View(model);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.Find(model.Username, model.Password);

                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
                IdentityResult result = userManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Error while changing the password.");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult ChangeProfile()
        {
            MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
            ChangeProfile profile = new ChangeProfile();
            profile.FullName = user.FullName;
            profile.Bio = user.Bio;
            profile.BirthDate = user.BirthDate;

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(ChangeProfile model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
                user.FullName = model.FullName;
                user.BirthDate = model.BirthDate;
                user.Bio = model.Bio;

                IdentityResult result = userManager.Update(user);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Profile saved successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Error while saving profile.");
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}