using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class FormationReasonDecline
    {
        [Required(ErrorMessage = "Введите причину отказа оформить заказ")]
        public string Reason { set; get; }
    }
}
