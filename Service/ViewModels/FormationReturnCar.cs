using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class FormationReturnCar
    {
        [Required(ErrorMessage = "Введите дату и время возврата автомобиля")]
        [DataType(DataType.DateTime)]
        public DateTime ReturnTime { set; get; }
    }
}
