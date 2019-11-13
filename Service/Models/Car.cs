using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель автомобиля
    public class Car
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Brand { set; get; }
        public string QulityClass { set; get; }
        public double Price { set; get; }
        public bool AvailabilityNow { set; get; }

        public virtual ICollection<Order> Orders { set;get; }
    }
}
