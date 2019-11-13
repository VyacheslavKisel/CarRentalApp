using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель причины отказа в аренде автомобиля
    public class ReasonRejection
    {
        [Key]
        [ForeignKey("Order")]
        public int Id { set; get; }
        public string Reason { set; get; }
        public virtual Order Order { set; get; }
    }
}
