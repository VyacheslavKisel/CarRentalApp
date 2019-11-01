using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Service;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class OrderController : Controller
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

        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult Order(int id)
        {
            string nameCurrentUser = User.Identity.Name;
            ApplicationUser currentUser = UserManager.FindByName(nameCurrentUser);
            ViewBag.UserId = currentUser.Id;
            ViewBag.CarId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Order(Order order)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Car car = db.Cars.Find(order.CarId);
                    car.AvailabilityNow = false;
                    db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                    TimeSpan span = order.FinalDate.Subtract(order.StartDate);
                    int numberDays = (int)span.TotalDays;
                    order.Invoice = numberDays * car.Price;
                    order.InvoiceMessage = false;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [Authorize(Roles = "manager, admin")]
        public ActionResult DetailsOrders()
        {
            var orders = _db.Orders;
            return View(orders.ToList());
        }

        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public ActionResult DetailsOrder(int id)
        {
            Order order = _db.Orders.Find(id);
            return View(order);
        }

        [HttpPost]
        public ActionResult DetailsOrder(AcceptModel model)
        {
            Order order = _db.Orders.Find(model.OrderId);
            if (model.Decision == "accept")
            {
                order.OrderStatusId = 3;
                order.InvoiceMessage = true;
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                order.ManagerProfileId = user.Id;
            }
            else if (model.Decision == "decline")
            {
                Car car = _db.Cars.Find(order.CarId);
                car.AvailabilityNow = true;
                _db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                order.OrderStatusId = 2;
                order.InvoiceMessage = false;
                ReasonRejection reason = new ReasonRejection { Id = model.OrderId };
                reason.Reason = model.Reason;
                _db.ReasonRejections.Add(reason);
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                order.ManagerProfileId = user.Id;
            }
            else if(model.Decision == "paid")
            {
                order.OrderStatusId = 4;
                order.InvoiceMessage = false;
            }
            else if(model.Decision == "received")
            {
                order.OrderStatusId = 5;
            }
            else if(model.Decision == "return")
            {
                ReturnCar returnCar = new ReturnCar { Id = model.OrderId };
                returnCar.ReturnTime = model.ReturnTime;
                _db.ReturnCars.Add(returnCar);
                Car car = _db.Cars.Find(order.CarId);
                _db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                if (model.Description != null)
                {
                    DamageCar damageCar = new DamageCar { Id = model.OrderId };
                    damageCar.Description = model.Description;
                    damageCar.CostRepair = model.CostRepair;
                    damageCar.InvoiceMessage = true;
                    damageCar.UnderRepairNow = true;
                    _db.DamageCars.Add(damageCar);
                    order.OrderStatusId = 6;
                }
                else
                {
                    order.OrderStatusId = 7;
                    car.AvailabilityNow = true;
                }
            }
            else if(model.Decision == "debtRepaid")
            {
                order.ReturnCar.DamageCar.InvoiceMessage = false;
            }
            _db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("DetailsOrders", "Order");
        }
    }
}