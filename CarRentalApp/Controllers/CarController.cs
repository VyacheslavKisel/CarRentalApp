using Service;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace CarRentalApp.Controllers
{
    // Контроллер для работы с автомобилями
    [Authorize(Roles = "admin")]
    public class CarController : Controller
    {
        private readonly ICarDataSource _carDataSource;

        public CarController()
        {
            _carDataSource = new CarDataSource();
        }

        [HttpGet]
        public ActionResult CreateCar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCar(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                Car car = new Car
                {
                    Title = model.Title,
                    Brand = model.Brand,
                    QulityClass = model.QulityClass,
                    Price = model.Price
                };
                car.AvailabilityNow = true;
                await _carDataSource.AddCarAsync(car);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditCar(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int carId = (int)id;
            Car car = await _carDataSource.GetCarAsync(carId);
            CarViewModel carViewModel = new CarViewModel(car.Id, car.Title, car.Brand, car.QulityClass, car.Price);
            if (car != null)
            {
                return View(carViewModel);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditCar(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                Car car = await _carDataSource.GetCarAsync(model.Id);
                car.Title = model.Title;
                car.Brand = model.Brand;
                car.QulityClass = model.QulityClass;
                car.Price = model.Price;
                car.AvailabilityNow = true;
                await _carDataSource.UpdateCarAsync(car);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DeleteCar(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int carId = (int)id;
            Car car = await _carDataSource.GetCarAsync(carId);
            if (car != null)
            {
                CarViewModel carViewModel = new CarViewModel(car.Id, car.Title, car.Brand, car.QulityClass, car.Price);
                return View(carViewModel);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("DeleteCar")]
        public async Task<ActionResult> DeleteCarConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            int carId = (int)id;
            await _carDataSource.DeleteCarAsync(carId);

            return RedirectToAction("Index", "Home");
        }
    }
}