using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель статуса заказа
    public class OrderStatus
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public virtual ICollection<Order> Orders { set; get; }
    }
}
