using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Service.Models;
using Microsoft.AspNet.Identity;

namespace Service.EF
{
    // Инициализатор базы данных
    public class ApplicationContextInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            var adminRole = new ApplicationRole { Name = "admin" };
            var managerRole = new ApplicationRole { Name = "manager" };
            var userRole = new ApplicationRole { Name = "user" };

            roleManager.Create(adminRole);
            roleManager.Create(managerRole);
            roleManager.Create(userRole);

            var admin = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
            string password = "_Aab12";
            var result = userManager.Create(admin, password);

            if(result.Succeeded)
            {
                userManager.AddToRole(admin.Id, adminRole.Name);
            }

            var user = new ApplicationUser { Email = "user@gmail.com", UserName = "user@gmail.com" };
            string passwordUser = "_Aab12";
            var resultUser = userManager.Create(user, passwordUser);

            if (resultUser.Succeeded)
            {
                userManager.AddToRole(user.Id, userRole.Name);
            }
            context.ClientProfiles.Add(new ClientProfile { Id = user.Id });

            var manager = new ApplicationUser { Email = "manager@gmail.com", UserName = "manager@gmail.com" };
            string passwordManager = "_Aab12";
            var resultManager = userManager.Create(manager, passwordManager);

            if (resultManager.Succeeded)
            {
                userManager.AddToRole(manager.Id, managerRole.Name);
            }
            context.ManagerProfiles.Add(new ManagerProfile { Id = manager.Id });

            context.Cars.Add(new Car { Title = "Volkswagen Jetta", Brand = "Volkswagen", QulityClass = "Стандарт", Price = 1300, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Hyundai Accent", Brand = "Hyundai", QulityClass = "Эконом", Price = 1000, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Mazda 6", Brand = "Mazda", QulityClass = "Бизнес", Price = 1600, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Nissan X-Trail", Brand = "Nissan", QulityClass = "Внедорожник", Price = 1750, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Toyota Avalon", Brand = "Toyota", QulityClass = "Премиум", Price = 3800, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Citroen С1", Brand = "Citroen", QulityClass = "Эконом", Price = 800, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Volkswagen Tiguan", Brand = "Volkswagen", QulityClass = "Внедорожник", Price = 1700, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Skoda Octavia A7", Brand = "Skoda", QulityClass = "Стандарт", Price = 1450, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "BMW X3", Brand = "BMW", QulityClass = "Премиум", Price = 4000, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Hyundai i10", Brand = "Hyundai", QulityClass = "Эконом", Price = 950, AvailabilityNow = true });
            context.Cars.Add(new Car { Title = "Hyundai Sonata", Brand = "Hyundai", QulityClass = "Бизнес", Price = 1500, AvailabilityNow = true });

            context.OrderStatuses.Add(new OrderStatus { Name = "Новый" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Отклоненный" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Согласованный" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Оплаченный" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Автомобиль у клиента" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Автомобиль возвращен с повреждениями, долг клиента" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Закрытый, автомобиль возвращен" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Закрытый, клиент погасил долг" });

            base.Seed(context);
        }
    }
}
