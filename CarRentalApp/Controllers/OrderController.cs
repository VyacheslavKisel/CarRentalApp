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
using System.Threading.Tasks;

namespace CarRentalApp.Controllers
{
    // Контроллер для работы с заказами
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

        private readonly IOrderDataSource _orderDataSource;
        private readonly ICarDataSource _carDataSource;
        private readonly IReasonRejectionDataSource _reasonRejectionDataSource;
        private readonly IReturnCarDataSource _returnCarDataSource;
        private readonly IDamageCarDataSource _damageCarDataSource;

        public OrderController()
        {
            _orderDataSource = new OrderDataSource();
            _carDataSource = new CarDataSource();
            _reasonRejectionDataSource = new ReasonRejectionDataSource();
            _returnCarDataSource = new ReturnCarDataSource();
            _damageCarDataSource = new DamageCarDataSource();
        }

        public OrderController(IOrderDataSource orderDataSource)
        {
            _orderDataSource = orderDataSource;
        }

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
        public async Task<ActionResult> Order(OrderCreature orderCreature)
        {
            if (ModelState.IsValid)
            {
                Car car = await _carDataSource.GetCarAsync(orderCreature.CarId);
                car.AvailabilityNow = false;
                await _carDataSource.UpdateCarAsync(car);
                Order order = new Order
                {
                    PassportData = orderCreature.PassportData,
                    AvailabilityDriver = orderCreature.AvailabilityDriver,
                    StartDate = orderCreature.StartDate,
                    FinalDate = orderCreature.FinalDate,
                    CarId = orderCreature.CarId,
                    OrderStatusId = orderCreature.OrderStatusId,
                    ClientProfileId = orderCreature.ClientProfileId
                };
                TimeSpan span = orderCreature.FinalDate.Subtract(orderCreature.StartDate);
                int numberDays = (int)span.TotalDays;
                order.Invoice = numberDays * car.Price;
                order.InvoiceMessage = false;
                await _orderDataSource.AddOrderAsync(order);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> DetailsOrders()
        {
            var orders = await _orderDataSource.GetOrdersAsync();
            List<OrderListArticle> ordersListDisplay = new List<OrderListArticle>();
            foreach (var item in orders)
            {
                ordersListDisplay.Add(
                    new OrderListArticle
                    {
                        Id = item.Id,
                        CarId = item.CarId,
                        PassportData = item.PassportData,
                        StartDate = item.StartDate,
                        FinalDate = item.FinalDate,
                        AvailabilityDriver = item.AvailabilityDriver,
                        NameOrderStatus = item.OrderStatus.Name,
                    }
                 );
                if (item.ManagerProfileId != null)
                {
                    ordersListDisplay[ordersListDisplay.Count - 1].ManagerEmail = item.ManagerProfile.ApplicationUser.Email;
                }
            }
            ViewBag.ManagerEmail = User.Identity.Name;
            return View(ordersListDisplay);
        }

        [Authorize(Roles = "manager, admin")]
        [HttpGet]
        public async Task<ActionResult> DetailsOrder(int id)
        {
            Order order = await _orderDataSource.GetOrderAsync(id);
            OrderDisplay orderDisplay = new OrderDisplay
            {
                Id = order.Id,
                PassportData = order.PassportData,
                StartDate = order.StartDate,
                FinalDate = order.FinalDate,
                AvailabilityDriver = order.AvailabilityDriver,
                CarId = order.CarId,
                NameOrderStatus = order.OrderStatus.Name,
            };
            return View(orderDisplay);
        }

        [HttpPost]
        public async Task<ActionResult> DetailsOrder(AcceptModel model)
        {
            Order order = await _orderDataSource.GetOrderAsync(model.OrderId);
            if (model.Decision == "accept")
            {
                order.OrderStatusId = 3;
                order.InvoiceMessage = true;
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                order.ManagerProfileId = user.Id;
            }
            else if (model.Decision == "decline")
            {
                Car car = await _carDataSource.GetCarAsync(order.CarId);
                car.AvailabilityNow = true;
                await _carDataSource.UpdateCarAsync(car);
                order.OrderStatusId = 2;
                order.InvoiceMessage = false;
                ReasonRejection reason = new ReasonRejection { Id = model.OrderId };
                reason.Reason = model.Reason;
                await _reasonRejectionDataSource.AddReasonRejectionAsync(reason);
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                order.ManagerProfileId = user.Id;
            }
            else if (model.Decision == "paid")
            {
                order.OrderStatusId = 4;
                order.InvoiceMessage = false;
            }
            else if (model.Decision == "received")
            {
                order.OrderStatusId = 5;
            }
            else if (model.Decision == "return")
            {
                ReturnCar returnCar = new ReturnCar { Id = model.OrderId };
                returnCar.ReturnTime = model.ReturnTime;
                await _returnCarDataSource.AddReturnCarAsync(returnCar);
                if (model.Description != null)
                {
                    DamageCar damageCar = new DamageCar { Id = model.OrderId };
                    damageCar.Description = model.Description;
                    damageCar.CostRepair = model.CostRepair;
                    damageCar.InvoiceMessage = true;
                    damageCar.UnderRepairNow = true;
                    await _damageCarDataSource.AddDamageCarAsync(damageCar);
                    order.OrderStatusId = 6;
                }
                else
                {
                    order.OrderStatusId = 7;
                    Car car = await _carDataSource.GetCarAsync(order.CarId);
                    car.AvailabilityNow = true;
                    await _carDataSource.UpdateCarAsync(car);
                }
            }
            else if (model.Decision == "debtRepaid")
            {
                order.OrderStatusId = 8;
                order.ReturnCar.DamageCar.InvoiceMessage = false;
            }
            await _orderDataSource.UpdateOrderAsync(order);
            return RedirectToAction("DetailsOrders", "Order");
        }



        //[HttpGet]
        //public async Task<ActionResult> ReasonDecline()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> ReasonDecline()
        //{
        //    return View();
        //}
    }
}