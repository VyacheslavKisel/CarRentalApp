using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель заказа
    public class Order
    {
        public int Id { set; get; }
        public string PassportData { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime FinalDate { set; get; }
        public bool AvailabilityDriver { set; get; }

        public bool InvoiceMessage { set; get; }
        public double Invoice { set; get; }

        public int CarId { set; get; }
        public virtual Car Car { set; get; }

        public int? OrderStatusId { set; get; }
        public virtual OrderStatus OrderStatus { set; get; }

        [MaxLength(128)]
        public string ClientProfileId { set; get; }
        public virtual ClientProfile ClientProfile { set; get; }

        [MaxLength(128)]
        public string ManagerProfileId { set; get; }
        public virtual ManagerProfile ManagerProfile { set; get; }

        public virtual ReturnCar ReturnCar { set; get; }

        public virtual ReasonRejection ReasonRejection { set; get; }
    }
}
