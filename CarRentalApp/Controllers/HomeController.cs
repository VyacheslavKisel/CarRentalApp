using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Service.ViewModels;
using System.Threading.Tasks;

namespace CarRentalApp.Controllers
{
    // Контроллер для работы с основными данными
    public class HomeController : Controller
    {
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

        private readonly ICarDataSource _carDataSource;
        private readonly IDamageCarDataSource _damageCarDataSource;
        private readonly IClientProfileDataSource _clientProfileDataSource;

        public HomeController()
        {
            _carDataSource = new CarDataSource();
            _damageCarDataSource = new DamageCarDataSource();
            _clientProfileDataSource = new ClientProfileDataSource();
        }

        public async Task<ActionResult> Index(CarsListViewModel model)
        {
            List<Car> cars = (List<Car>)await _carDataSource.GetCarsAsync();
            List<Car> carsSelected = null;

            if (model.BrandsSelected != null && model.QualityClassesSelected != null)
            {
                foreach (var item in cars)
                {
                    foreach (var i in model.BrandsSelected)
                    {
                        foreach (var j in model.QualityClassesSelected)
                        {
                            if (item.Brand == i && item.QulityClass == j)
                            {
                                if (carsSelected == null)
                                {
                                    carsSelected = new List<Car>();
                                }
                                carsSelected.Add(item);
                            }
                        }
                    }
                }
            }
            else if (model.BrandsSelected != null)
            {
                foreach (var item in cars)
                {
                    foreach (var i in model.BrandsSelected)
                    {
                        if (item.Brand == i)
                        {
                            if (carsSelected == null)
                            {
                                carsSelected = new List<Car>();
                            }
                            carsSelected.Add(item);
                        }
                    }
                }
            }
            else if (model.QualityClassesSelected != null)
            {
                foreach (var item in cars)
                {
                    foreach (var i in model.QualityClassesSelected)
                    {
                        if (item.QulityClass == i)
                        {
                            if (carsSelected == null)
                            {
                                carsSelected = new List<Car>();
                            }
                            carsSelected.Add(item);
                        }
                    }
                }
            }
            if (carsSelected == null)
            {
                carsSelected = cars;
            }
            if (model.SortPrice == "SortAscending")
            {
                carsSelected = carsSelected.OrderBy(p => p.Price).ToList();
            }
            else if (model.SortPrice == "SortDescending")
            {
                carsSelected = carsSelected.OrderByDescending(p => p.Price).ToList();
            }
            if (model.SortTitle == "SortAscending")
            {
                carsSelected = carsSelected.OrderBy(p => p.Title).ToList();
            }
            else if (model.SortTitle == "SortDescending")
            {
                carsSelected = carsSelected.OrderByDescending(p => p.Title).ToList();
            }
            HashSet<string> brands = new HashSet<string>();
            HashSet<string> qualityClasses = new HashSet<string>();
            brands.Add("Все");
            qualityClasses.Add("Все");
            var temp = cars.Select(p => new
            {
                Brand = p.Brand,
                QualityClass = p.QulityClass
            });
            foreach (var item in temp)
            {
                brands.Add(item.Brand);
                qualityClasses.Add(item.QualityClass);
            }
            CarsListViewModel clvm = new CarsListViewModel
            {
                Cars = carsSelected,
                Brands = brands.ToList(),
                QualityClasses = qualityClasses.ToList()
            };
            return View(clvm);
        }

        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> DamageCars()
        {
            var damageCars = await _damageCarDataSource.GetDamageCarsAsync();
            List<DamageCarArticleList> damageCarsArticleList = new List<DamageCarArticleList>();
            foreach (var item in damageCars)
            {
                damageCarsArticleList.Add(
                    new DamageCarArticleList
                    {
                        Id = item.Id,
                        Description = item.Description,
                        CostRepair = item.CostRepair,
                        InvoiceMessage = item.InvoiceMessage,
                        UnderRepairNow = item.UnderRepairNow,
                        ManagerEmail = item.ReturnCar.Order.ManagerProfile.ApplicationUser.Email
                    }
                );
            }
            ViewBag.ManagerEmail = User.Identity.Name;
            return View(damageCarsArticleList);
        }

        [HttpPost]
        public async Task<ActionResult> DamageCars(DamageCarArticleList damageCarArticleList)
        {
            var damageCar = await _damageCarDataSource.GetDamageCarAsync(damageCarArticleList.Id);
            damageCar.UnderRepairNow = false;
            await _damageCarDataSource.UpdateDamageCarAsync(damageCar);
            if (damageCarArticleList.UnderRepairNow == false)
            {
                Car car = await _carDataSource.GetCarAsync(damageCar.ReturnCar.Order.CarId);
                car.AvailabilityNow = true;
                await _carDataSource.UpdateCarAsync(car);
                await _damageCarDataSource.UpdateDamageCarAsync(damageCar);
            }
            return RedirectToAction("DamageCars", "Home");
        }

        [Authorize(Roles = "manager")]
        public ActionResult ManagerPanel()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        public async Task<ActionResult> PersonalArea()
        {
            string currentUserId = User.Identity.GetUserId();
            ClientProfile profile = await _clientProfileDataSource.GetClientProfileAsync(currentUserId);
            InvoiceForClient invoiceForClient = new InvoiceForClient();
            foreach (var item in profile.Orders)
            {
                if (item.InvoiceMessage)
                {
                    invoiceForClient.InvoiceRental = item.Invoice;
                }
                if (item.ReturnCar != null && item.ReturnCar.DamageCar != null)
                {
                    if (item.ReturnCar.DamageCar.InvoiceMessage)
                    {
                        invoiceForClient.CostRepair = item.ReturnCar.DamageCar.CostRepair;
                    }
                }
            }
            return View(invoiceForClient);
        }
    }
}