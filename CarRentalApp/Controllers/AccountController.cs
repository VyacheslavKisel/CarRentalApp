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
using NLog;

namespace CarRentalApp.Controllers
{
    // Контроллер для работы с учетными записями
    public class AccountController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

        private readonly IClientProfileDataSource _clientProfileDataSource;
        private readonly IManagerProfileDataSource _managerProfileDataSource;

        public AccountController()
        {
            _clientProfileDataSource = new ClientProfileDataSource();
            _managerProfileDataSource = new ManagerProfileDataSource();
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
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else if(await UserManager.IsLockedOutAsync(user.Id))
                {
                    return View("Lockout");
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
                    if (String.IsNullOrEmpty(returnUrl))
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
            if (ModelState.IsValid)
            {
                string userRole = "user";
                ApplicationRole role = RoleManager.FindByName(userRole);
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName };
                Microsoft.AspNet.Identity.IdentityResult result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, role.Name);
                    ClientProfile profile = new ClientProfile { Id = user.Id };
                    _clientProfileDataSource.AddClientProfileAsync(profile);

                    logger.Info("Был зарегистрирован новый клиент");

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
        [HttpGet]
        public ActionResult RegisterManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterManager(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string userRole = "manager";
                ApplicationRole role = RoleManager.FindByName(userRole);
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName };
                Microsoft.AspNet.Identity.IdentityResult result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, role.Name);
                    ManagerProfile profile = new ManagerProfile { Id = user.Id };
                    _managerProfileDataSource.AddManagerProfileAsync(profile);

                    logger.Info("Был зарегистрирован новый менеджер");

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
            return View(UserManager.Users);
        }

        //Блокировка пользователя
        [Authorize(Roles = "admin")]
        public ActionResult BlockUserAccount(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            UserBlockData userBlockData = new UserBlockData()
            {
                UserId = applicationUser.Id,
                Email = applicationUser.Email,
                LockoutEnabled = applicationUser.LockoutEnabled,
            };
            if(applicationUser.LockoutEndDateUtc == null)
            {
                userBlockData.DateTimeBlock = DateTime.UtcNow;
            }
            else
            {
                userBlockData.DateTimeBlock = (DateTime)applicationUser.LockoutEndDateUtc;
            }
            return View(userBlockData);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult BlockUserAccount(UserBlockData userBlockData)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.SetLockoutEnabled(userBlockData.UserId, true);
                if (result.Succeeded)
                {
                    result = UserManager.SetLockoutEndDate(userBlockData.UserId, (DateTimeOffset)userBlockData.DateTimeBlock);
                    ApplicationUser applicationUser = UserManager.FindById(userBlockData.UserId);
                    logger.Info($"Был заблокирован пользователь, у которого логин {applicationUser.UserName} и почта {applicationUser.Email}");
                }
                return RedirectToAction("DataUsers");
            }
            return View();
        }

        // Разблокировка пользователей
        [Authorize(Roles = "admin")]
        public ActionResult UnBlockUserAccount(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            UserBlockData userBlockData = new UserBlockData()
            {
                UserId = applicationUser.Id,
                Email = applicationUser.Email,
                LockoutEnabled = applicationUser.LockoutEnabled,
            };
            if (applicationUser.LockoutEndDateUtc == null)
            {
                userBlockData.DateTimeBlock = DateTime.UtcNow;
            }
            else
            {
                userBlockData.DateTimeBlock = (DateTime)applicationUser.LockoutEndDateUtc;
            }
            return View(userBlockData);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult UnBlockUserAccount(UserBlockData userBlockData)
        {
            var result = UserManager.SetLockoutEnabled(userBlockData.UserId, true);
            if (result.Succeeded)
            {
                result = UserManager.SetLockoutEndDate(userBlockData.UserId, DateTimeOffset.UtcNow);
                result = UserManager.SetLockoutEnabled(userBlockData.UserId, false);
                ApplicationUser applicationUser = UserManager.FindById(userBlockData.UserId);
                logger.Info($"Был разблокирован пользователь, у которого логин {applicationUser.UserName} и почта {applicationUser.Email}");
            }
            return RedirectToAction("DataUsers");
        }
    }
}