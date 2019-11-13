using Microsoft.AspNet.Identity.EntityFramework;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Service.EF
{
    // Контекст базы данных
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("RentalCarContext")
        {
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new ApplicationContextInitializer());
        }

        public DbSet<Car> Cars { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<ClientProfile> ClientProfiles { set; get; }
        public DbSet<ManagerProfile> ManagerProfiles { set; get; }
        public DbSet<OrderStatus> OrderStatuses { set; get; }
        public DbSet<ReturnCar> ReturnCars { set; get; }
        public DbSet<DamageCar> DamageCars { set; get; }
        public DbSet<ReasonRejection> ReasonRejections { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientProfile>()
                .HasMany(p => p.Orders)
                .WithRequired(prop => prop.ClientProfile)
                .HasForeignKey(s => s.ClientProfileId);

            modelBuilder.Entity<ManagerProfile>()
                .HasMany(p => p.Orders)
                .WithRequired(prop => prop.ManagerProfile)
                .HasForeignKey(s => s.ManagerProfileId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
