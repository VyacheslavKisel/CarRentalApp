using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class CarController : Controller
    {
        ApplicationContext _db = new ApplicationContext();

        [HttpGet]
        public ActionResult CreateCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Cars.Add(car);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditCar(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Car car = _db.Cars.Find(id);
            if (car != null)
            {
                return View(car);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(car).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteCar(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Car car = _db.Cars.Find(id);
            if (car != null)
            {
                return View(car);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("DeleteCar")]
        public ActionResult DeleteCarConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Car car = _db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            _db.Cars.Remove(car);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}