using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using Service;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class AccountController : Controller
    {
        ApplicationContext _db = new ApplicationContext();
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if(String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                string userRole = "user";
                ApplicationRole role = RoleManager.FindByName(userRole);
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName };
                Microsoft.AspNet.Identity.IdentityResult result = UserManager.Create(user, model.Password);
                if(result.Succeeded)
                {   
                    UserManager.AddToRole(user.Id, role.Name);
                    ClientProfile profile = new ClientProfile { Id = user.Id };
                    _db.ClientProfiles.Add(profile);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult RegisterManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterManager(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                string userRole = "manager";
                ApplicationRole role = RoleManager.FindByName(userRole);
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName };
                Microsoft.AspNet.Identity.IdentityResult result = UserManager.Create(user, model.Password);
                if(result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, role.Name);
                    ManagerProfile profile = new ManagerProfile { Id = user.Id };
                    _db.ManagerProfiles.Add(profile);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DataUsers()
        {
            var users = UserManager.Users;
            return View(users);
        }
    }
}