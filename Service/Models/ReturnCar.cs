using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель автомобиля, который вернули с аренды
    public class ReturnCar
    {
        [Key]
        [ForeignKey("Order")]
        public int Id { set; get; }
        public DateTime ReturnTime { set; get; }
        public virtual Order Order { set; get; }
        public virtual DamageCar DamageCar { set; get; }
    }
}
