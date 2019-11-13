using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель поврежденного автомобиля
    public class DamageCar
    {
        [Key]
        [ForeignKey("ReturnCar")]
        public int Id { set; get; }
        public string Description { set; get; }
        public double CostRepair { set; get; }
        public bool InvoiceMessage { set; get; }
        public bool UnderRepairNow { set; get; }
        public virtual ReturnCar ReturnCar { set; get; }
    }
}
