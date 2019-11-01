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

namespace CarRentalApp.Controllers
{
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

        ApplicationContext _db = new ApplicationContext();
        public ActionResult Index(CarsListViewModel model)
        {
            List<Car> cars = _db.Cars.ToList();
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
                            if(carsSelected == null)
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
            if(model.SortPrice == "SortAscending")
            {
                carsSelected = carsSelected.OrderBy(p => p.Price).ToList();
            }
            else if(model.SortPrice == "SortDescending")
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
            var temp = _db.Cars.Select(p => new
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
        public ActionResult DamageCars()
        {
            var damageCars = _db.DamageCars.ToList();
            return View(damageCars);
        }

        [HttpPost]
        public ActionResult DamageCars(DamageCar damageCarModel)
        {
            var damageCar = _db.DamageCars.Find(damageCarModel.Id);
            damageCar.UnderRepairNow = false;
            _db.Entry(damageCar).State = EntityState.Modified;
            if (damageCarModel.UnderRepairNow == false)
            {
                Car car = _db.Cars.Find(damageCar.ReturnCar.Order.CarId);
                car.AvailabilityNow = true;
                _db.Entry(damageCar).State = EntityState.Modified;
            }
            _db.SaveChanges();
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
        public ActionResult PersonalArea()
        {
            string currentUserId = User.Identity.GetUserId();
            ClientProfile profile = _db.ClientProfiles.Find(currentUserId);
            return View(profile.Orders);
        }
    }
}